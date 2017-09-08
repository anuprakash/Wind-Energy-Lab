﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapPointer : MonoBehaviour {

    [Header("GUI components")]
    [SerializeField] private Image infoPanel;
    private TextMeshProUGUI textPro;
    [Space]
    public Sprite[] pointerSprites;
	SpriteRenderer spriteRend;
    LevelManager levelManager;
    private Vector2 spritePos;
    private Vector2 panelPos;
    private Vector3 mousePos;

    void Start()
	{

        levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
		spriteRend = GetComponent<SpriteRenderer>();
		this.spriteRend.sprite = pointerSprites[0];
        
        // get sprite position
        spritePos = GetComponent<Transform>().position;

        //panel 
        textPro = infoPanel.GetComponentInChildren<TextMeshProUGUI>();
        EnableInfoPanel(false);

	}

	void OnMouseEnter()
	{
		this.spriteRend.sprite = pointerSprites[1];
        PlacePanelNextToSprite();
        EnableInfoPanel(true);
        
	}

	void OnMouseExit()
	{
		this.spriteRend.sprite = pointerSprites[0];
        EnableInfoPanel(false);
	}

    private void OnMouseDown()
    {
        levelManager.LoadNextLevel();
    }

    void PlacePanelNextToSprite()
    {
        mousePos = Input.mousePosition;
        panelPos.x = mousePos.x;
        panelPos.y = mousePos.y + 130.0f;
        infoPanel.transform.position = panelPos;
    }

    void EnableInfoPanel( bool visible )
    {
        if(visible == true)
        {
            infoPanel.enabled = true;
            textPro.enabled = true;
        }
        else
        {
            infoPanel.enabled = false;
            textPro.enabled = false;
        }
        
    }

}