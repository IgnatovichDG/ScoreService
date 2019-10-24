namespace ScoreService.Services
{
    /// <summary>
    ///     Генератор хеш-а для безопасности.
    /// </summary>
    public interface IHashGenerator
    {
        /// <summary>
        ///     Вычислить криптоустойчивый хеш для защиты данных.
        /// </summary>
        /// <param name="source">Исходная строка для вычисления хеша.</param>
        /// <returns>Строка, содержащая хеш в BASE-64.</returns>
        string ComputeHash(string source);

        /// <summary>
        ///     Вычислить криптоустойчивый хеш для пароля пользователя.
        /// </summary>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="salt">Соль для генерации хеша.</param>
        /// <returns>Строка, содержащая хеш в BASE-64.</returns>
        string ComputePasswordHash(string password,string salt);
    }
}