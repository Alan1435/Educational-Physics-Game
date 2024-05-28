using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partColorChange : MonoBehaviour
{
    [SerializeField] GameObject placer;
    private SpriteRenderer sprite;
    private int amountNumber;
    private bool isPlaceable;
    private bool isHolding;

    private PartPlacer partPlacer ;
    // Start is called before the first frame update
    void Start()
    {
        sprite = placer.GetComponent<SpriteRenderer>();
        partPlacer = placer.GetComponent<PartPlacer>() ;
    }

    // Update is called once per frame
    void Update()
    {
        isHolding = placer.GetComponent<PartPlacer>().getIsHolding();
        if(isHolding)
        {
            amountNumber = partPlacer.getPartButtonScript().GetPartNumber();
            isPlaceable = placer.GetComponent<RaycastAndMath>().IsPlaceable() && partPlacer.SelectedPartWithinBoundsAtPos(placer.transform.position);

            if(amountNumber<=0 || !isPlaceable)
            {
                sprite.color = new Color(1,0,0,1);
                Debug.Log("color red");
            }
            else if (isPlaceable)
            {
                sprite.color = new Color(0,1,0,1);
                Debug.Log("color green");
            }
        }
    }
}
