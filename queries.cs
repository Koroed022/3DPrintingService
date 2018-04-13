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
			MySqlCommand getId = new MySqlCommand("SELECT id FROM users WHERE password=@pass, login=@login", MySQlConnection);
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

		public static bool Registration(string login, string pass, int accessLvl, string full_name, string phone_numb)
		{
			MySQlConnection.Open();
			MySqlCommand getId = new MySqlCommand("SELECT id FROM users WHERE login=@login", MySQlConnection);
			getId.Parameters.AddWithValue("login", login);

			if (getId.ExecuteScalar() == null)
			{
				MySqlCommand setUser = new MySqlCommand("INSERT INTO users (login, password, accessLvl, fullName, phoneNumb)VALUES(@login, @password, @accessLvl, @fullName, @phoneNumb)", MySQlConnection);
				setUser.Parameters.AddWithValue("login", login);
				setUser.Parameters.AddWithValue("password", pass);
				setUser.Parameters.AddWithValue("accessLvl", accessLvl);
				setUser.Parameters.AddWithValue("fullName", full_name);
				setUser.Parameters.AddWithValue("phoneNumb", phone_numb);
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

		public static bool AddOrder(int idUser, int id_mat, float volume, float cost, string finishDate, string address,
			string tranc, int deliv_type, string hash)
		{
			try
			{
				MySQlConnection.Open();
				MySqlCommand addOrder = new MySqlCommand("INSERT INTO orders (idUsers, idMaterials, volume, " +
					"totalCost, startDate, finishDate, delivAddress, transicNumb, delivType, trackNumb, hashNumb)" +
				"VALUES(@idUsers, @idMaterials, @volume,@totalCost, @startDate, @finishDate, @delivAddress, " +
				"@transicNumb, @delivType, @trackNumb, @hashNumb)", MySQlConnection);
				addOrder.Parameters.AddWithValue("idUsers", idUser);
				addOrder.Parameters.AddWithValue("idMaterials", id_mat);
				addOrder.Parameters.AddWithValue("volume", volume);
				addOrder.Parameters.AddWithValue("totalCost", cost);
				addOrder.Parameters.AddWithValue("startDate", DateTime.Now);
				addOrder.Parameters.AddWithValue("finishDate", finishDate);
				addOrder.Parameters.AddWithValue("delivAddress", address);
				addOrder.Parameters.AddWithValue("delivType", deliv_type);
				addOrder.Parameters.AddWithValue("trackNumb", tranc);
				addOrder.Parameters.AddWithValue("hashNumb", hash);
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
		public static bool EditUser(string login, string password, int access_lvl, string full_name, string phone_numb)
		{
			MySQlConnection.Open();
			MySqlCommand findUser = new MySqlCommand("SELECT * FROM users WHERE login=@login", MySQlConnection);
			findUser.Parameters.AddWithValue("login", login);

			if (findUser.ExecuteScalar() == null)
			{
				MySQlConnection.Close();
				return false;
			}
			else
			{
				string[] user = new string[5];
				MySqlDataReader sqlReader = null;
				sqlReader = findUser.ExecuteReader();
				while (sqlReader.Read())
				{
					user[0] = sqlReader[1].ToString();
					user[1] = sqlReader[2].ToString();
					user[2] = sqlReader[3].ToString();
					user[3] = sqlReader[4].ToString();
					user[4] = sqlReader[5].ToString();
				}
				if (password != "")
					user[1] = password;

				if (access_lvl != 0)
					user[2] = access_lvl.ToString();

				if (full_name != "")
					user[3] = full_name;

				if (phone_numb != "")
					user[4] = phone_numb;

				MySqlCommand editUser = new MySqlCommand("UPDATE users SET password=@password, accessLvl=@accessLvl,fullName=@fullName,phoneNumb=@phoneNumb WHERE login=@login", MySQlConnection);
				editUser.Parameters.AddWithValue("password", user[1]);
				editUser.Parameters.AddWithValue("accessLvl", user[2]);
				editUser.Parameters.AddWithValue("fullName", user[3]);
				editUser.Parameters.AddWithValue("phoneNumb", user[4]);
				editUser.ExecuteNonQuery();
				MySQlConnection.Close();
				return true;
			}
		}
		public static bool EditOrder(string hash_number, string deliv_address, string track_number)
		{
			MySQlConnection.Open();
			MySqlCommand findOrder = new MySqlCommand("SELECT * FROM users WHERE hash=@hash", MySQlConnection);
			findOrder.Parameters.AddWithValue("hash", hash_number);

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

				MySqlCommand editUser = new MySqlCommand("UPDATE orders SET delivAddress=@delivAddress, trackNumb=@trackNumb WHERE idOrders=@idOrders", MySQlConnection);
				editUser.Parameters.AddWithValue("delivAddress", order[1]);
				editUser.Parameters.AddWithValue("trackNumb", order[2]);
				editUser.Parameters.AddWithValue("idOrders", order[0]);
				editUser.ExecuteNonQuery();
				MySQlConnection.Close();
				return true;
			}
		}


		//история
		//расширенный статус заказа (выполнен + доставка)
		public static string ShowOrders_unreg(int hash_numb) { return ""; }

		//public static  ShowOrders_reg(int idUser)
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
