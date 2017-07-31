using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour {
    public int number; //The count it was added at

    public SelectorPanel panel; //The selector panel at this location
    public GridCell upCell;
    public GridCell downCell;
    public GridCell leftCell;
    public GridCell rightCell;
    
}
