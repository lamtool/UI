using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using AutoAndroid;
using Sunny.Subdy.Common.Logs;

namespace Sunny.Subdy.Server
{
    public class SubdyHttpServer
    {
        private HttpListener _listener;
        private bool _isRunning = false;
        private const string Port = "8686";
        private const string ServerUrl = "http://localhost:" + Port + "/";
        private ApiRouter _router;
        public SubdyHttpServer()
        {
            _router = new ApiRouter();
            RegisterRoutes(); // Đăng ký tất cả các API endpoint
        }

        /// <summary>
        /// Bắt đầu lắng nghe các yêu cầu HTTP.
        /// </summary>
        public async Task StartServer()
        {
            if (_isRunning)
            {
                LogManager.Info("Server is already running.");
                return;
            }

            _listener = new HttpListener();
            _listener.Prefixes.Add(ServerUrl);

            try
            {
                _listener.Start();
                _isRunning = true;
                LogManager.Info($"Listening for requests on {ServerUrl}");

                // Chạy vòng lặp lắng nghe yêu cầu trong một Task riêng
                _ = Task.Run(async () =>
                {
                    while (_isRunning)
                    {
                        HttpListenerContext context = null;
                        try
                        {
                            context = await _listener.GetContextAsync();
                            // Chuyển yêu cầu cho router xử lý
                            await _router.RouteRequest(context);
                        }
                        catch (HttpListenerException ex)
                        {
                            // Lỗi khi listener bị dừng (ví dụ: StopServer được gọi)
                            if (ex.ErrorCode == 995) // The I/O operation has been aborted...
                            {
                                LogManager.Info("HttpListener was closed gracefully.");
                            }
                            else
                            {
                                LogManager.Error(ex);
                            }
                            break; // Thoát vòng lặp khi có lỗi nghiêm trọng hoặc listener bị đóng
                        }
                        catch (ObjectDisposedException ex)
                        {
                            // Xử lý khi listener bị hủy sau khi StopServer được gọi
                            LogManager.Info("HttpListener object disposed gracefully.");
                            break;
                        }
                        catch (Exception ex)
                        {
                            // Đây là nơi bạn bắt các lỗi tổng quát trong quá trình xử lý yêu cầu
                            LogManager.Error(ex);

                            if (context != null) // Đảm bảo context không null
                            {
                                try
                                {
                                    await ApiRouter.SendJsonResponse(context.Response, ApiResponse<object>.ErrorResponse($"Unhandled server error: {ex.Message}"), HttpStatusCode.InternalServerError);
                                }
                                catch (Exception innerEx) // THÊM LOG Ở ĐÂY
                                {
                                    LogManager.Error(innerEx);
                                }
                                finally
                                {
                                    try { context.Response.Close(); } catch { }
                                }
                            }
                        }
                    }
                });
            }
            catch (HttpListenerException ex)
            {
                LogManager.Error(ex);
                if (ex.ErrorCode == 5) // Access Denied
                {

                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        /// <summary>
        /// Dừng lắng nghe các yêu cầu HTTP.
        /// </summary>
        public void StopServer()
        {
            if (_isRunning)
            {
                _isRunning = false;
                if (_listener != null && _listener.IsListening)
                {
                    _listener.Stop();
                    _listener.Close();
                    LogManager.Info("HTTP Server stopped.");
                }
                _listener = null;
            }
        }

        /// <summary>
        /// Đăng ký tất cả các API endpoint và các hàm xử lý tương ứng.
        /// </summary>
        private void RegisterRoutes()
        {
            // Route cho đường dẫn gốc
            _router.Get("/", HandleRootRequest);

            // Các route cho API Devices
            _router.Get("/devices", GetDevices); // Đã đổi tên route

            // Route cho yêu cầu thay đổi thiết bị theo ID (ID dạng chuỗi)
            _router.Get("/{id}/Change", HandleChangeRequest); // VẪN DÙNG HandleChangeRequest

            LogManager.Info("All API routes registered.");
        }

        /// <summary>
        /// Xử lý yêu cầu GET tới "/{id}/Change".
        /// </summary>
        private async Task HandleChangeRequest(HttpListenerContext context, Dictionary<string, string> routeParams)
        {
            LogManager.Info("Handling /{id}/Change request.");

            if (!routeParams.TryGetValue("id", out string idValue))
            {
                await ApiRouter.SendJsonResponse(
                    context.Response,
                    ApiResponse<object>.ErrorResponse("ID parameter not found in URL."),
                    HttpStatusCode.BadRequest
                );
                return;
            }

            LogManager.Info($"Extracted ID: {idValue}");

            try
            {
                var client = new ADBClient(idValue);
                client.Connect();

                if (!client.ChangInfo("", false))
                {
                    await ApiRouter.SendJsonResponse(
                        context.Response,
                        ApiResponse<object>.ErrorResponse("Thay đổi thông tin thiết bị thất bại!"),
                        HttpStatusCode.InternalServerError
                    );
                    return;
                }

                await Task.Delay(3000); // Chờ thiết bị cập nhật

                var deviceName = client.GetDeviceName();
                if (string.IsNullOrEmpty(deviceName))
                {
                    await ApiRouter.SendJsonResponse(
                        context.Response,
                        ApiResponse<object>.ErrorResponse("Thay đổi thiết bị thất bại! (Tên thiết bị trống)"),
                        HttpStatusCode.InternalServerError
                    );
                    return;
                }

                // Trả về thông tin thiết bị (tùy chỉnh thêm nếu cần)
                var responseData = new DeviceRespone
                {
                    Serial = idValue,
                    NameDevice = client.Device?.NameDevice,
                    Status = deviceName,
                    OS = client.Device?.OS ?? "Unknown"
                };

                await JsonSerializer.SerializeAsync(
                    context.Response.OutputStream,
                    ApiResponse<DeviceRespone>.SuccessResponse(responseData, "Thay đổi thông tin thiết bị thành công!"),
                    MyJsonContext.Default.ApiResponseDeviceRespone
                );
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                await ApiRouter.SendJsonResponse(
                    context.Response,
                    ApiResponse<object>.ErrorResponse($"Lỗi xử lý yêu cầu thay đổi thiết bị: {ex.Message}"),
                    HttpStatusCode.InternalServerError
                );
            }
        }

        private async Task WriteJsonResponse<T>(HttpListenerContext context, bool success, string message, T data = default, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            try
            {
                var payload = new JsonResponse<T>
                {
                    success = success,
                    message = message,
                    data = data
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                string json = JsonSerializer.Serialize(payload, options);
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength64 = buffer.Length;

                await context.Response.OutputStream.WriteAsync(buffer);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }
        /// <summary>
        /// Xử lý yêu cầu GET tới đường dẫn gốc "/".
        /// </summary>
        private async Task HandleRootRequest(HttpListenerContext context, Dictionary<string, string> routeParams)
        {
            LogManager.Info("Handling GET / request.");
            await ApiRouter.SendJsonResponse(context.Response, ApiResponse<string>.SuccessResponse("Welcome to the C# HTTP Server!", "Server is running."));
        }

        /// <summary>
        /// Xử lý yêu cầu GET tới "/devices" để lấy danh sách thiết bị.
        /// </summary>
        private async Task GetDevices(HttpListenerContext context, Dictionary<string, string> routeParams)
        {
            LogManager.Info("Handling GET /devices request.");
            var devices = new List<DeviceModel>();

            try
            {
                var lines = ADBHelper.GetDevices(); // Giả định ADBHelper.GetDevices trả về IEnumerable<string>
                if (lines != null && lines.Any())
                {
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        var client = new ADBClient(line.Trim()); // trim để loại bỏ khoảng trắng dư thừa
                        devices.Add(client.Device); // Giả định client.Device là một DeviceModel
                    }

                }
                var selectedDevices = devices.Select(d => new DeviceRespone
                {
                    Serial = d.Serial, // Ánh xạ Id của DeviceModel sang Serial
                    NameDevice = d.NameDevice,
                    OS = d.OS // Đảm bảo DeviceModel có thuộc tính OS
                }).ToList();
                await JsonSerializer.SerializeAsync(context.Response.OutputStream,
     ApiResponse<List<DeviceRespone>>.SuccessResponse(selectedDevices, "Danh sách thiết bị đã lấy thành công."),
     MyJsonContext.Default.ApiResponseListDeviceRespone);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                await ApiRouter.SendJsonResponse(context.Response, ApiResponse<object>.ErrorResponse($"Lỗi khi lấy danh sách thiết bị: {ex.Message}"), HttpStatusCode.InternalServerError);
            }
        }

    }
}
