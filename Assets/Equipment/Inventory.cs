using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	// Singleton
	private static Inventory instance;
	public List<Armor> armorList = new List<Armor>();
	public List<Weapon> weaponList = new List<Weapon>();

	// Construct
	private Inventory() {}

	//  Instance
	public static Inventory Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType (typeof(Inventory)) as  Inventory;
			return instance;
		}
	}
	// Do something here, make sure this  is public so we can access it through our Instance.
	public void  DoSomething()
	{
		print (armorList[0].physicalProtection);
	}
}
