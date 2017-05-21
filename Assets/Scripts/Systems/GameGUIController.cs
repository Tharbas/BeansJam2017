using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;





public class GameGUIController : MonoBehaviour {

	public Canvas gameCanvas; 
	public GameObject[] guiPanels;
	private GameObject ingameGui;
	private GameObject gameEndWindow;
	private GameObject winningMafiosoPanel;
	private GameObject winningCopPanel;

	void Start()
	{
		guiPanels = GameObject.FindGameObjectsWithTag ("guipanel");
	}

}
