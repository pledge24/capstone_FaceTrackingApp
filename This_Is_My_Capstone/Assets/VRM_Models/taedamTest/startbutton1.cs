using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startbutton1 : MonoBehaviour
{
	public void SceneChange(){
		SceneManager.LoadScene("Bodytest");
	}
}
