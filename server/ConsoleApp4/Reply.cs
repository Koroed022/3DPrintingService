using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Классы ответа к Диме после запроса к бд
public class ReplyUser
{
	public string command, error;
	public string idUsers, login, password, accessLvl, fullName, phoneNumb;
}

public class RelpyOrders
{
	public string command, error;

	public string user_id;

	public string idMaterials, volume, total_cost, startDate, finishDate,  transicNumb, trackNumb, hashNumb;
	public string delivType, delivAddress, delivStatus, delivFullStatus;
	public RelpyOrders(string in_user)
	{
		this.user_id = in_user;
	}
}

public class ReplyMaterial
{
	public string command, error;

	public string material_id, material;
	public string costPerCube;
	public string idPrinter;
	public string residue;
	public ReplyMaterial(string in_material)
	{
		this.material_id = in_material;
	}
}
