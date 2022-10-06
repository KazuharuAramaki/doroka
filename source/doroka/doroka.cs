using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace project_ESCD_r2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" --- start running Address HostName Checker ---\n");

            // string cpt = "DESKTOP-DUNLICH";
            // //ローカルコンピュータ上のイベントログをすべて取得する
            // System.Diagnostics.EventLog[] logs = System.Diagnostics.EventLog.GetEventLogs(cpt);
            // //次のようにコンピュータ名を指定することも出来る
            // //logs = System.Diagnostics.EventLog.GetEventLogs("myMachine");
            // foreach (System.Diagnostics.EventLog log in logs)
            // {
            //     //イベントログの名前を出力する
            //     Console.WriteLine(("Log: " + log.Log));
            //     //ログエントリをすべて取得する
            //     // foreach (System.Diagnostics.EventLogEntry entry in log.Entries)
            //     // {
            //     //     //ログエントリのメッセージを出力する
            //     //     Console.WriteLine("\tEntry: " + entry.Message);
            //     // }
            // }

            // string logName = "Windows PowerShell";
            // if (EventLog.Exists(logName)) {
            //     Console.WriteLine((logName + " is Exist"));
            // } else {
            //     Console.WriteLine((logName + " is NOT Exist"));
            // }

            string logmsg = IPAddressCheck();

            //ReadLog("Microsoft-Windows-Dhcp-Client/Admin")

            WriteLog(logmsg);

            Console.ReadLine();
        }

        static void ReadLog(string logName){
            string machineName = ".";
            EventLog read_log = new EventLog(logName, machineName);
            StringBuilder sb = new StringBuilder();
                foreach (EventLogEntry entry in read_log.Entries)
            {
                sb.Length = 0;
                sb.Append("Type = ");
                sb.Append(entry.EntryType);
                sb.Append(" Source = ");
                sb.Append(entry.Source);
                sb.Append(" Time = ");
                sb.Append(entry.TimeGenerated);
                Console.WriteLine(sb.ToString());
            }
        }

        static void WriteLog(string msg){

            Console.WriteLine(" --- start running Create Send EventLog ---\n");

            string cpt = ".";             // コンピュータ名
            string log = "Application";   // イベント・ログ名
            string src = "Original-IPAddress and HostName";  // イベント・ソース名

            Console.WriteLine(" log_name: " + log);
            Console.WriteLine(" src_name: " + src);
            Console.WriteLine(" log_msg: " + msg + "\n");

            if (!EventLog.SourceExists(src))
            {
                EventSourceCreationData data = new EventSourceCreationData(src, log);
                EventLog.CreateEventSource(data);
            }

            EventLog evlog = new EventLog(log, cpt, src);
            evlog.WriteEntry(msg, EventLogEntryType.Information);

            Console.WriteLine(" --- end running Create Send EventLog ---\n");
        }

        static string IPAddressCheck(){
            // ホスト名を取得する
            string hostname = Dns.GetHostName();
            //Console.WriteLine("HostName : " + hostname);

            string localIP = string.Empty;
            Console.WriteLine(" try connect to 8.8.8.8");
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);

                Console.WriteLine("  --> OK");

                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();

            }
            //Console.WriteLine("IP Address = " + localIP);
            string msg = "HostName : " + hostname + ", IP Address = " + localIP;

            Console.WriteLine(" result");
            Console.WriteLine("   hostname: " + hostname);
            Console.WriteLine("   localIP: " + localIP + "\n");

            Console.WriteLine(" --- end running Address HostName Checker ---");

            return msg;
        }
    }
}
