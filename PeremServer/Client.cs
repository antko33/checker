using System;
using System.Net.Sockets;
using System.Text;

namespace PeremServer
{
    /**
	 *Представляет отдельное подключение 
	 */

    class Client
    {
        public TcpClient _client;

        public Client(TcpClient client)
        {
            this._client = client;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = _client.GetStream();
                byte[] data = new byte[64];

                while (true)
                {
                    //Проба
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);

                    string message = builder.ToString();

                    Console.WriteLine(message);

                    message = message.Trim().ToUpper();
                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    /**
                     * <TODO>
                     * Приём от клиента номера ПЕ (ГОТОВ)
                     * Отправка клиенту задания в ответ
                     * Приём от клиента результатов (ЗАВЕРШЁН)
                     * Разрыв соединения
                     * </TODO>
                     */
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                stream?.Close(); //Ругается на недостижимый код, потом пройдёт
                _client?.Close();
            }

        }
    }
}
