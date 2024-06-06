using Grpc.Core;

namespace GrpcService.Services
{
    public class CalculatorService : Calculator.CalculatorBase
    {
        public override Task<WorkResponse> Work(WorkRequest request, ServerCallContext context)
        {
            int? result = request.Operation switch
            {
                "+" => request.A + request.B,
                "-" => request.A - request.B,
                "*" => request.A * request.B,
                "/" => request.A / request.B,
                _ => null,
            };

            if (result is not null)
            {
                return Task.FromResult(new WorkResponse
                {
                    Result = (int)result
                });
            }
            else
                {
                return Task.FromResult(new WorkResponse
                {
                    Error = "Invalid operation"
                });
            }

        }
    }
}
