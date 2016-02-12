namespace Invisual.AppCore.Logging
{
	public enum LogLevel
	{
		/// <summary>
		/// For highly detailed logging, not output in production.
		/// </summary>
		Trace,

		/// <summary>
		/// Development and debugging messages, not output in production. E.g. method entry/exit, key events, decision points.
		/// </summary>
		Debug,

		/// <summary>
		/// Important app behaviour, to be recorded in production.
		/// System/session lifecycle (app start, stop, login, logout).
		/// Boundary events (remote API/database calls).
		/// Business events (failed login/payment, user audit messages).
		/// </summary>
		Info,

		/// <summary>
		/// Issues which need looking at, but don't require immediate human intervention.
		/// </summary>
		Warning,

		/// <summary>
		/// Serious errors, customers likely to be affected.
		/// </summary>
		Error
	}
}
