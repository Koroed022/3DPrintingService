using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


public class MySQLProc 
{
	static string connectionDB = "server=localhost;user=root;database=Keksik;password=1234;SslMode=none;";
	static MySqlConnection MySQlConnection = new MySqlConnection(connectionDB);
	ReplyUser reply_user;
	RelpyOrders reply_orders;
	ReplyMaterial reply_material;

	public MySQLProc(ReplyUser user)
	{
		this.reply_user = user;
	}
	public MySQLProc(RelpyOrders orders)
	{
		this.reply_orders = orders;
	}
	public MySQLProc(ReplyMaterial material)
	{
		this.reply_material = material;
	}

	public string ReturnUsersID()
	{
		MySQlConnection.Open();
		MySqlCommand getId = new MySqlCommand("SELECT id FROM users WHERE password=@pass, login=@login", MySQlConnection);
		getId.Parameters.AddWithValue("login", reply_user.login);
		getId.Parameters.AddWithValue("pass", reply_user.password);

		if (getId.ExecuteScalar() == null)
		{
			int error = 0;
			reply_user.error = error.ToString();
			return reply_user.error;
		}
		else
		{
			reply_user.idUsers = getId.ExecuteScalar().ToString();
		}
		MySQlConnection.Close();
		return reply_user.idUsers;
	}

	public bool Registration(string login, string pass, int accessLvl, string full_name, string phone_numb)
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
	public static List<string[]> ShowOrders_unreg(int hash_numb) { return ""; }

	public static List<string[]> ShowOrders_reg(int idUser)
	{
		MySQlConnection.Open();
		MySqlCommand showOrders = new MySqlCommand("SELECT idMaterials, totalCost, startDate, finishDate, delivAddress, delivType, delivStatus, delivFullStatus, trackNumb, hashNumb FROM orders WHERE idUser=@idUser", MySQlConnection);
		showOrders.Parameters.AddWithValue("idUser", idUser);
		List<string[]> orders_list = new List<string[]>();
		int cnt = 0;
		MySqlDataReader sqlReader = showOrders.ExecuteReader();
		while (sqlReader.Read())
		{
			orders_list.Add(new string[10]);
			orders_list[orders_list.Count - 1][0] = sqlReader[0].ToString();
			orders_list[orders_list.Count - 1][1] = sqlReader[1].ToString();
			orders_list[orders_list.Count - 1][2] = sqlReader[2].ToString();
			orders_list[orders_list.Count - 1][3] = sqlReader[3].ToString();
			orders_list[orders_list.Count - 1][4] = sqlReader[4].ToString();
			orders_list[orders_list.Count - 1][5] = sqlReader[5].ToString();
			orders_list[orders_list.Count - 1][6] = sqlReader[6].ToString();
			orders_list[orders_list.Count - 1][7] = sqlReader[7].ToString();
			orders_list[orders_list.Count - 1][8] = sqlReader[8].ToString();
			orders_list[orders_list.Count - 1][9] = sqlReader[9].ToString();
			cnt++;
		}
		for (int i = 0; i < cnt; i++)
		{
			MySqlCommand getMaterialName = new MySqlCommand("SELECT material FROM materials WHERE idMaterial=@idMaterial", MySQlConnection);
			getMaterialName.Parameters.AddWithValue("idMaterials", orders_list[cnt][0]);
			orders_list[cnt][0] = getMaterialName.ExecuteScalar().ToString();
		}
		return orders_list;
	}

	public static bool EditMaterial() { return false; }
}
