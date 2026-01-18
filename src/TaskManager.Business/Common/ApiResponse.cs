using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Business.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string>? Errors { get; set; }
        public T? Data { get; set; }

        public ApiResponse() { }

        // Constructor for successful response with data
        public ApiResponse(T data, string mensaje = "")
        {
            Success = true;
            Message = mensaje;
            Data = data;
            Errors = null;
        }

        // Constructor for error response
        public ApiResponse(string mensaje, IEnumerable<string>? errors = null)
        {
            Success = false;
            Message = mensaje;
            Data = default;
            Errors = errors;
        }
    }
}
