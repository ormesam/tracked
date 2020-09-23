namespace Tracked.Auth {
    public enum GoogleClientErrorType {
        SignInUnknownError,
        SignInKeychainError,
        NoSignInHandlersInstalledError,
        SignInHasNoAuthInKeychainError,
        SignInCanceledError,
        SignInDefaultError,
        SignInApiNotConnectedError,
        SignInInvalidAccountError,
        SignInNetworkError,
        SignInInternalError,
        SignInRequiredError,
        SignInFailedError
    }
}
