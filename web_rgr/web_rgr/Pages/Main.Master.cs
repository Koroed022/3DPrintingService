using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Sockets;

namespace web_rgr.Pages {
	public partial class Main : System.Web.UI.MasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			
		}

		protected void Login(object sender, EventArgs e) {
			try {
				string res = @"{""login"":""" + log_login.Text + @""",""pass"":""" + log_pass.Text + @"""}";
				string IP = "10.241.75.194";
				TcpClient client = new TcpClient();
				client.Connect(IP, 9999);
				Byte[] data = System.Text.Encoding.Unicode.GetBytes(res);
				NetworkStream stream = client.GetStream();
				stream.Write(data, 0, data.Length);
				data = new Byte[1000];
				String responseData;
				Int32 bytes = stream.Read(data, 0, data.Length);
				responseData = System.Text.Encoding.Unicode.GetString(data, 0, bytes);
				stream.Close();
				client.Close();
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
	}
}