using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DishManager : MonoBehaviour
{
    public List<DishInfo> allDishes = new List<DishInfo>();
    private List<string> dishOnCollide = new List<string>();
    
    private GameObject dishSlot;
    private GameObject dishBase;
    public GameObject inventoryContent;
    public Scrollbar scrollBar;

    // For drag and drop physics
    public LayerMask m_DragLayers;          // Layer dedicated for loose items
    public bool isDishOnCharacter = false;
    private TargetJoint2D m_TargetJoint;
    private Rigidbody2D body;               // Loose item held by mouse - rigidbody2D component

    [Range(0.0f, 100.0f)]
    public float m_Damping = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_Frequency = 5.0f;

    private CharacterSystem _characterSystem;
    private CoinSystem _coinSystem;

    private void Awake()
    {
        // Reference the scripts from game objects
        try 
        { 
            _characterSystem = GameObject.FindGameObjectWithTag("CharacterSystem").GetComponent<CharacterSystem>(); 
            _coinSystem = GameObject.FindGameObjectWithTag("CoinSystem").GetComponent<CoinSystem>();
        }
        catch (UnityException) {};
    }

    private void Start()
    {
        // Load all allDishes from the Resources/DishInfo folder
        allDishes = Resources.LoadAll<DishInfo>("DishInfo").ToList();
        
        // Reference the prefab objects
        dishSlot = Resources.Load("Prefabs/dishSlot", typeof(GameObject)) as GameObject;
        dishBase = Resources.Load("Prefabs/dishBase", typeof(GameObject)) as GameObject;

        // Load the dish slots on the inventory content
        foreach (DishInfo dishInfo in allDishes.OrderBy(item => item.name).ToList())
        {
            // Instantiate dish slot
            GameObject newDishSlot = Instantiate(dishSlot, inventoryContent.transform);

            // Set children to each slot to display the dish's sprite, name, buy point, and description
            newDishSlot.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = dishInfo.sprite;                              // Dish image sprite
            newDishSlot.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = dishInfo.name;                                   // Dish name text
            newDishSlot.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = string.Format("{0:0.00}", dishInfo.buyPoint);                // Price text
            newDishSlot.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = dishInfo.description;                                        // Description text
            
            // Set children to each slot to display the macronutrient attributes
            newDishSlot.transform.GetChild(3).GetComponent<Slider>().value = dishInfo.hungerRestoration / 100.0f;                                  // Hunger restore slider value
            newDishSlot.transform.GetChild(3).GetChild(3).GetComponent<Text>().text = string.Format("+{0:0.0}", dishInfo.hungerRestoration);       // Hunger restore points
            newDishSlot.transform.GetChild(4).GetComponent<Slider>().value = dishInfo.energy / 100.0f;                                             // Energy slider value
            newDishSlot.transform.GetChild(4).GetChild(3).GetComponent<Text>().text = string.Format("+{0:0.0}", dishInfo.energy);                  // Energy points
            newDishSlot.transform.GetChild(5).GetComponent<Slider>().value = dishInfo.fats / 100.0f;                                               // Fats slider value
            newDishSlot.transform.GetChild(5).GetChild(3).GetComponent<Text>().text = string.Format("+{0:0.0}", dishInfo.fats);                    // Fats points

            // Add an event trigger to each button
            EventTrigger clickTrigger = newDishSlot.transform.GetChild(1).GetChild(0).GetComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerDown
            };

            // Add a listener SpawnDish(dishInfo) to each button for its functionality
            clickEvent.callback.AddListener((data) => { SpawnDish(dishInfo); });
            clickTrigger.triggers.Add(clickEvent);
        }
        StartCoroutine(UpdateDisplay());
    }

    private IEnumerator UpdateDisplay()
    {
        inventoryContent.GetComponent<RectTransform>().sizeDelta = new Vector2
            (inventoryContent.GetComponent<RectTransform>().sizeDelta.x, 
            (inventoryContent.GetComponent<GridLayoutGroup>().cellSize.y + inventoryContent.GetComponent<GridLayoutGroup>().spacing.y) * Mathf.CeilToInt(allDishes.Count));
        yield return new WaitForSeconds(0.01f);
        scrollBar.value = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Execute if a dish is collided in the main character
        isDishOnCharacter = true;
        dishOnCollide.Add(collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Execute if a dish is away from the main character
        isDishOnCharacter = false;
        dishOnCollide.Remove(collision.gameObject.name);
    }

    private void Update()
    {
        // Loose item behavior
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Set the position of a selected dish based on the cursor pointed
        // if the mouse left click is in hold position
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D collider = Physics2D.OverlapPoint(mousePosition, m_DragLayers);
            if (!collider)
                return;

            body = collider.attachedRigidbody;
            if (!body)
                return;

            if (m_TargetJoint)
                Destroy(m_TargetJoint);
            

            m_TargetJoint = body.gameObject.AddComponent<TargetJoint2D>();
            m_TargetJoint.dampingRatio = m_Damping;
            m_TargetJoint.frequency = m_Frequency;
            m_TargetJoint.anchor = m_TargetJoint.transform.InverseTransformPoint(mousePosition);
            _characterSystem.DialogueChat(5);
        }
        // Drop the selected dish if the mouse left click is not in hold position
        else if (Input.GetMouseButtonUp(0))
        {
            Destroy(m_TargetJoint);
            m_TargetJoint = null;
            _characterSystem.DialogueChat(0);
            
            if (isDishOnCharacter)
                CheckDish();

            if (body) 
                Destroy(body.gameObject);

            isDishOnCharacter = false;
            body = null;
            return;
        }

        if (m_TargetJoint)
            m_TargetJoint.target = mousePosition;
    }

    public void SpawnDish(DishInfo dishInfo)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Instantiate a selected dish when it is dragged from the dish slot
        GameObject newDish = Instantiate(dishBase, mousePosition, Quaternion.identity);

        // Set the name, local scale, sprite, and collider size of the spawned dish
        newDish.name = dishInfo.name;

        if (dishInfo.sprite == null)
            return;

        newDish.transform.localScale = new Vector3(dishInfo.scaleX, dishInfo.scaleY, 0);
        newDish.GetComponent<SpriteRenderer>().sprite = dishInfo.sprite;
        newDish.GetComponent<BoxCollider2D>().size = new Vector2(dishInfo.colliderSizeX, dishInfo.colliderSizeY);
    }

    public void CheckDish()
    {
        float _c_hunger = PlayerPrefs.GetFloat("C-Hunger", 50);
        float _c_energy = PlayerPrefs.GetFloat("C-Energy", 50);
        float _c_fats = PlayerPrefs.GetFloat("C-Fats", 0);
        float _globalCoins = PlayerPrefs.GetFloat("GlobalCoins", _coinSystem.initialCoins);
        
        // Reiterate until the dish is matched with the dish collided in the character
        for (int i = 0; i < allDishes.Count; i++)
        {
            if (dishOnCollide[0] == allDishes[i].name)
            {
                // Breaks if the player has not enough money
                if (_globalCoins < allDishes[i].buyPoint)
                {
                    _characterSystem.DialogueChat(4);
                    return;
                }
                
                // Breaks if the character's hunger is more than 95 points
                if (_c_hunger >= 95)
                {
                    _characterSystem.DialogueChat(0);
                    return;
                }

                // Breaks if the character's fats can't handle a dish due to more fats
                if (_c_fats + allDishes[i].fats >= 100)
                {
                    _characterSystem.DialogueChat(2);
                    return;
                }
                
                _characterSystem.DialogueChat(3);
                _characterSystem.IncreaseHunger(allDishes[i].hungerRestoration);
                _characterSystem.IncreaseEnergy(allDishes[i].energy);
                _characterSystem.IncreaseFats(allDishes[i].fats);
                
                _coinSystem.DecreaseCoins(allDishes[i].buyPoint);
            }
        }
    }

}
