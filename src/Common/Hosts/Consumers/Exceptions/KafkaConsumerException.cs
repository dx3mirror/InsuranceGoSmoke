namespace InsuranceGoSmoke.Common.Consumers.Exceptions
{
    /// <summary>
    /// Исключение обработки события.
    /// </summary>
    public class KafkaConsumerException : Exception
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="KafkaConsumerException" />
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public KafkaConsumerException(string message) : base(message) 
        { 
        }

        /// <summary>
        /// Создаёт экземпляр <see cref="KafkaConsumerException" />
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="exception">Исключение.</param>
        public KafkaConsumerException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
