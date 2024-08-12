using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class FactoryUIHolder : SingletonMonoBehavior<FactoryUIHolder>
{
    [SerializeField] ProcessingSlot[] processing = new ProcessingSlot[9];
    [SerializeField] CompletedSlot[] completed = new CompletedSlot[9];
    [SerializeField] Button addButton;
    [SerializeField] TextMeshProUGUI price;

    [SerializeField] RectTransform rectTrans;
    
    public Factory currentFactory;

    protected override void Awake()
    {
        base.Awake();
        addButton.onClick.AddListener(OnClickAddButton);
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Init(Factory factory, IEnumerable<ProductData> process, IEnumerable<ProductData> complete)
    {
        Queue<ProductData> processData = process != null ? new Queue<ProductData>(process) : new Queue<ProductData>();
        Queue<ProductData> completeData = complete != null ? new Queue<ProductData>(complete) : new Queue<ProductData>();
        
        currentFactory = factory;

        rectTrans.anchoredPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        
        for (int i = 0; i < factory.queueCapacity; i++)
        {
            processing[i].UnloadAllData();
            completed[i].UnloadAllData();
            
            //Fix distribution of conflict between processing queue and completed queue later
            if (processData.Count != 0) processing[i].Init(processData.Dequeue());
            if (completeData.Count != 0) completed[i].Init(completeData.Dequeue());
            
            processing[i].gameObject.SetActive(true);
            completed[i].gameObject.SetActive(true);
            //End of fix
        }
        
        if (currentFactory.queueCapacity < 9) addButton.gameObject.SetActive(true);
        price.text = (3 + currentFactory.queueCapacity * 2).ToString();
        
        OpenCurtain();
        gameObject.SetActive(true);
    }

    void OpenCurtain()
    {
        UICurtain.Instance.Transparent();
        UICurtain.Instance.AssignOnClickOnce(() =>
        {
            gameObject.SetActive(false);
            UICurtain.Instance.TurnOff();
        });
    }

    void OnDisable()
    {
        for (int i = 0; i < 9; i++)
        {
            processing[i].Hide();
            completed[i].Hide();
            
            processing[i].gameObject.SetActive(false);
            completed[i].gameObject.SetActive(false);
        }
        
        addButton.gameObject.SetActive(false);
    }

    void OnClickAddButton()
    {
        currentFactory.queueCapacity++;
        currentFactory.ReloadFactoryUIHolder();
        
        if (currentFactory.queueCapacity == 9) addButton.gameObject.SetActive(false);
    }
}
