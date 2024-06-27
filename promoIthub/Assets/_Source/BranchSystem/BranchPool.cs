using System.Collections.Generic;
using UnityEngine;

namespace BranchSystem
{
    public class BranchPool
    {
        public  List<GameObject> Branches { get;  set; }


        public void InitPool(GameObject branch,int max,GameObject parent)
        {
            Branches = new List<GameObject>();
            for (var i = 0; i < max; i++)
            {
                var branchInstance = GameObject.Instantiate(branch, parent.transform);
                ReturnToPool(branchInstance);
            }

        }

        

        public bool TryGetFromPool(out GameObject branchInstance)
        {
        
            branchInstance = null;
       
            if (Branches.Count > 0)
            {
            
                branchInstance = Branches[0];
                branchInstance.SetActive(true);
                Branches.RemoveAt(0);
                return true;
            }

            return false;
        }

        public void ReturnToPool(GameObject enemyInstance)
        {
            enemyInstance.SetActive(false);
            Branches.Add(enemyInstance);
           
        }
    }
}