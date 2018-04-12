using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQL_Processing
{
	class MySQLProc
	{
		public static string connectionDB = "server=localhost;user=root;database=Keksik;password=1234;SslMode=none;";
		static MySqlConnection MySQlConnection = new MySqlConnection(connectionDB);

		public static int ReturnUsersID(string login, string pass)
		{
			int id = 0;
			MySQlConnection.Open();
			MySqlCommand getId = new MySqlCommand("SELECT id FROM users WHERE password=@pass, email=@login", MySQlConnection);
			getId.Parameters.AddWithValue("login", login);
			getId.Parameters.AddWithValue("pass", pass);

			if (getId.ExecuteScalar() == null)
				return -1;
			else
			{
				id = (Int32)getId.ExecuteScalar();
			}
			MySQlConnection.Close();
			return id;
		}

		public static bool Registration(string login, string pass, string credits, string phoneNumb)
		{
			MySQlConnection.Open();
			MySqlCommand getId = new MySqlCommand("SELECT id FROM users WHERE email=@login", MySQlConnection);
			getId.Parameters.AddWithValue("login", login);

			if (getId.ExecuteScalar() == null)
			{
				MySqlCommand setUser = new MySqlCommand("INSERT INTO users (Email, Password, AccessLvl, Credits, PhoneNumber)VALUES(@Email, @Password, @AccessLvl, @Credits, @PhoneNumber)", MySQlConnection);
				setUser.Parameters.AddWithValue("Email", login);
				setUser.Parameters.AddWithValue("Password", pass);
				setUser.Parameters.AddWithValue("AccessLvl", 0);
				setUser.Parameters.AddWithValue("Credits", credits);
				setUser.Parameters.AddWithValue("PhoneNumber", phoneNumb);
				setUser.ExecuteNonQuery();
				MySQlConnection.Close();
				return true;
			}
			else
			{
				MySQlConnection.Close();
				return false;
			}
		}

		public static bool AddOrder(int idUser, int id_mat, float volume, float cost, string FinishDate, string address,
			string tranc, int deliv_type, string Hash)
		{
			try
			{
				MySQlConnection.Open();
				MySqlCommand addOrder = new MySqlCommand("INSERT INTO orders (idUsers, idMaterials, Volume, " +
					"TotalCost, StartDate, FinishDate, DelivAddress, TransicNumb, DelivType, TrackNumb, HashNumb)" +
				"VALUES(@idUsers, @idMaterials, @Volume,@TotalCost, @StartDate, @FinishDate, @DelivAddress, " +
				"@TransicNumb, @DelivType, @TrackNumb, @HashNumb)", MySQlConnection);
				addOrder.Parameters.AddWithValue("idUsers", idUser);
				addOrder.Parameters.AddWithValue("idMaterials", id_mat);
				addOrder.Parameters.AddWithValue("Volume", volume);
				addOrder.Parameters.AddWithValue("TotalCost", cost);
				addOrder.Parameters.AddWithValue("StartDate", DateTime.Now);
				addOrder.Parameters.AddWithValue("FinishDate", FinishDate);
				addOrder.Parameters.AddWithValue("DelivAddress", address);
				addOrder.Parameters.AddWithValue("DelivType", deliv_type);
				addOrder.Parameters.AddWithValue("TrackNumb", tranc);
				addOrder.Parameters.AddWithValue("HashNumb", Hash);
				addOrder.ExecuteNonQuery();
				MySQlConnection.Close();
				return true;
			}
			catch
			{
				MySQlConnection.Close();
				return false;
			}
		}
		public static bool RemoveOrder(int idOrders)
		{
			try
			{
				MySQlConnection.Open();
				MySqlCommand delOrder = new MySqlCommand("DELETE FROM orders WHERE idOrders=@idOrders", MySQlConnection);
				delOrder.Parameters.AddWithValue("idOrders", idOrders);
				delOrder.ExecuteNonQuery();
				MySQlConnection.Close();
				return true;
			}
			catch
			{
				MySQlConnection.Close();
				return false;
			}
		}
		public static bool EditUser(string email, string password, int access_lvl, string credits, string phone_number)
		{
			MySQlConnection.Open();
			MySqlCommand findUser = new MySqlCommand("SELECT * FROM users WHERE email=@email", MySQlConnection);
			findUser.Parameters.AddWithValue("email", email);

			if (findUser.ExecuteScalar() == null)
			{
				MySQlConnection.Close();
				return false;
			}
			else
			{
				string[] user = new string[6];
				MySqlDataReader sqlReader = null;
				sqlReader = findUser.ExecuteReader();
				while (sqlReader.Read())
				{
					user[0] = sqlReader[0].ToString();
					user[1] = sqlReader[1].ToString();
					user[2] = sqlReader[2].ToString();
					user[3] = sqlReader[3].ToString();
					user[4] = sqlReader[4].ToString();
					user[5] = sqlReader[5].ToString();

				}
				if (password != "")
					user[2] = password;

				if (access_lvl != 0)
					user[3] = access_lvl.ToString();

				if (credits != "")
					user[4] = credits;

				if (phone_number != "")
					user[5] = phone_number;

				MySqlCommand editUser = new MySqlCommand("UPDATE users SET Password=@Password, AccessLvl=@AccessLvl,Credits=@Credits,PhoneNumber=@PhoneNumber WHERE email=@email", MySQlConnection);
				editUser.Parameters.AddWithValue("Password", user[2]);
				editUser.Parameters.AddWithValue("AccessLvl", user[3]);
				editUser.Parameters.AddWithValue("Credits", user[4]);
				editUser.Parameters.AddWithValue("PhoneNumber", user[5]);
				editUser.ExecuteNonQuery();
				MySQlConnection.Close();
				return true;
			}
		}
		public static bool EditOrder(string hash_number, string deliv_address, string track_number)
		{
			MySQlConnection.Open();
			MySqlCommand findOrder = new MySqlCommand("SELECT * FROM users WHERE hash_number=@hash_number", MySQlConnection);
			findOrder.Parameters.AddWithValue("hash_number", hash_number);

			if (findOrder.ExecuteScalar() == null)
			{
				MySQlConnection.Close();
				return false;
			}
			else
			{
				string[] order = new string[3];
				MySqlDataReader sqlReader = null;
				sqlReader = findOrder.ExecuteReader();
				while (sqlReader.Read())
				{
					order[0] = sqlReader[0].ToString();
					order[1] = sqlReader[6].ToString();
					order[2] = sqlReader[10].ToString();
				}
				if (deliv_address != "")
					order[6] = deliv_address;

				if (track_number != "")
					order[10] = track_number;

				MySqlCommand editUser = new MySqlCommand("UPDATE orders SET DelivAddress=@DelivAddress, TrackNumb=@TrackNumb WHERE idOrders=@idOrders", MySQlConnection);
				editUser.Parameters.AddWithValue("DelivAddress", order[1]);
				editUser.Parameters.AddWithValue("TrackNumb", order[2]);
				editUser.Parameters.AddWithValue("idOrders", order[0]);
				editUser.ExecuteNonQuery();
				MySQlConnection.Close();
				return true;
			}
		}

		//public static bool ShowOrders(int idUser)
		//{
		//	MySQlConnection.Open();
		//	MySqlCommand showOrders = new MySqlCommand("SELECT idOrders, idMaterials FROM orders WHERE idUser=@idUser", MySQlConnection);
		//	List<string[]> orders_list = new List<string[]>();
		//	MySqlDataReader sqlReader = null;
		//	while (sqlReader.Read())
		//	{
		//		orders_list.Add(new string[2]);
		//		orders_list[orders_list.Count - 1][0] = sqlReader[0].ToString();
		//		orders_list[orders_list.Count - 1][1] = sqlReader[0].ToString();

		//	}

		//	return true;
		//}
		public static bool EditMaterial() { return false; }

		static void Main(string[] args)
		{

		}
	}
}
