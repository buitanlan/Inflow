using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Users.Core.Exceptions;

internal class SignUpDisabledException() : InflowException("Sign up is disabled.");