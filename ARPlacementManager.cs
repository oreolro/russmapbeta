using UnityEngine;
using System.Collections.Generic;

public class ARPlacementManager : MonoBehaviour
{
    private GameObject modelToPlace;
    private GameObject spawnedModel;

    public void SetModelToPlace(GameObject prefab)
    {
        modelToPlace = prefab;
        if (spawnedModel != null) Destroy(spawnedModel);
    }

    private void Update()
    {
        if (modelToPlace == null) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (spawnedModel == null)
                    {
                        spawnedModel = Instantiate(modelToPlace, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        spawnedModel.transform.position = hit.point;
                    }
                }
            }
        }
    }
}