using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeApp : MonoBehaviour {
    [Header("Boids")]
    public Button boidsButton;
    public GameObject boidsPanel;
    public GameObject boidsCamera;
    [Space]
    [Header("Creature Generation")]
    public Button creatureButton;
    public GameObject creaturePanel;
    public GameObject creatureCamera;
    [Space]
    [Header("Ants")]
    public Button antsButton;
    public GameObject antsPanel;
    public GameObject antsCamera;

	void Start () {
        boidsButton.onClick.AddListener(ChangeToBoids);
        creatureButton.onClick.AddListener(ChangeToCreature);
        antsButton.onClick.AddListener(ChangeToAnts);
	}
	
    public void ChangeToBoids()
    {
        boidsPanel.SetActive(true);
        creaturePanel.SetActive(false);
        antsPanel.SetActive(false);

        Camera.main.transform.position = boidsCamera.transform.localPosition;
    }

    public void ChangeToCreature()
    {
        boidsPanel.SetActive(false);
        creaturePanel.SetActive(true);
        antsPanel.SetActive(false);

        Camera.main.transform.position = creatureCamera.transform.localPosition;
    }

    public void ChangeToAnts()
    {
        boidsPanel.SetActive(false);
        creaturePanel.SetActive(false);
        antsPanel.SetActive(true);

        Camera.main.transform.position = antsCamera.transform.localPosition;
    }
}
