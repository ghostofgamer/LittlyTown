using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private ItemPosition _angularTile;
    [SerializeField] private ItemPosition _endTile;
    [SerializeField] private ItemPosition _fullCrossroadsTile;
    [SerializeField] private ItemPosition _clearTile;
    [SerializeField] private ItemPosition[] _itemPositions;

    private Dictionary<string, ItemPosition> _tileConfigurations;

    private void Start()
    {
        _tileConfigurations = new Dictionary<string, ItemPosition>()
        {
            {"0000", _clearTile},
            {"1111", _fullCrossroadsTile},
            {"0100", _endTile},
            {"1100", _angularTile},
            // ...
        };

        Generation();
    }

    private void Generation()
    {
        foreach (ItemPosition itemPosition in _itemPositions)
        {
            if (!itemPosition.IsBusy)
            {
                string surroundingTiles = CheckSurroundingTiles(itemPosition);

                // var selectedTile = _tileConfigurations[surroundingTiles];

                ItemPosition selectedTile = Instantiate(_tileConfigurations[surroundingTiles],
                    itemPosition.transform.position, Quaternion.identity);
                
                
                if (selectedTile == _angularTile)
                {
                    switch (surroundingTiles)
                    {
                        case "1100":
                            selectedTile.transform.rotation = Quaternion.Euler(0, 90, 0);
                            break;
                        case "0010":
                            selectedTile.transform.rotation = Quaternion.Euler(0, -90, 0);
                            break;
                        case "0100":
                            selectedTile.transform.rotation = Quaternion.Euler(0, 180, 0);
                            break;
                        case "1000":
                            selectedTile.transform.rotation = Quaternion.Euler(0, 0, 0);
                            break;
                    }
                }
                
                
                /*if (selectedTile != null)
                {
                    ItemPosition newTile = Instantiate(selectedTile, itemPosition.transform.position, Quaternion.identity);
                }*/
            }
        }
    }

    private string CheckSurroundingTiles(ItemPosition itemPosition)
    {
        string surroundingTiles = "0000";

        if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy)
        {
            surroundingTiles = "1" + surroundingTiles.Substring(1);
            Debug.Log("Coordinats " + surroundingTiles);
        }

        if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy)
        {
            surroundingTiles = surroundingTiles.Substring(0, 2) + "1" + surroundingTiles.Substring(3);
            Debug.Log("Coordinats " + surroundingTiles);
        }

        if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy)
        {
            surroundingTiles = surroundingTiles.Substring(0, 1) + "1" + surroundingTiles.Substring(2);
            Debug.Log("Coordinats " + surroundingTiles);
        }

        if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy)
        {
            surroundingTiles = surroundingTiles.Substring(0, 3) + "1";
            Debug.Log("Coordinats " + surroundingTiles);
        }

        Debug.Log("Coordinats " + surroundingTiles);
        return surroundingTiles;
    }


    /*private void GenerateRoad()
   {
       // ...

       if (!tile.IsBusy)
       {
           string surroundingTiles = CheckSurroundingTiles(tile.transform.position);
           GameObject selectedTile = tileConfigurations[surroundingTiles];

           if (selectedTile != null)
           {
               GameObject newTile = Instantiate(selectedTile, tile.transform.position, Quaternion.identity);
               newTile.GetComponent<Tile>().IsBusy = true;

               // Вращение тайла в нужное положение
               if (selectedTile == turnTile)
               {
                   switch (surroundingTiles)
                   {
                       case "0001":
                           newTile.transform.rotation = Quaternion.Euler(0, 90, 0);
                           break;
                       case "0010":
                           newTile.transform.rotation = Quaternion.Euler(0, -90, 0);
                           break;
                       case "0100":
                           newTile.transform.rotation = Quaternion.Euler(0, 180, 0);
                           break;
                       case "1000":
                           newTile.transform.rotation = Quaternion.Euler(0, 0, 0);
                           break;
                   }
               }
               else if (selectedTile == deadEndTile)
               {
                   switch (surroundingTiles)
                   {
                       case "0011":
                           newTile.transform.rotation = Quaternion.Euler(0, 0, 0);
                           break;
                       case "0101":
                           newTile.transform.rotation = Quaternion.Euler(0, 90, 0);
                           break;
                       case "1001":
                           newTile.transform.rotation = Quaternion.Euler(0, -90, 0);
                           break;
                       case "1100":
                           newTile.transform.rotation = Quaternion.Euler(0, 180, 0);
                           break;
                   }
               }
           }
       }

       // ...
   }*/
}