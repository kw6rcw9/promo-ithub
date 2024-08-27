using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using PlayerSystem.TeleportSystem;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace BranchSystem
{
    public class BranchGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject parent;
        [SerializeField] private GameObject leftPrefab;
        [SerializeField] private GameObject rightPrefab;
        [SerializeField] private int maxPoolSize = 20;
        [SerializeField] private int restriction;
        [Inject] private TeleportPlayer _teleportPlayer;
        [Inject] private BranchPool _pool;
        private List<Branch> _genBranchList;
        private List<Branch> _prevGenBranchList;
        private Branch _leftBranch;
        private Branch _rightBranch;
        private bool _positionChanged = false;
        private Transform _startPoint;
        private float _yTrans = -3;
        private Branch _initialBranch;
        public void Init()
        {
            // leftPrefab.TryGetComponent(out Branch leftBranch);
            // _leftBranch = leftBranch;
            // _leftBranch.IsCentered = leftBranch.IsCentered;
            // _leftBranch.BranchSprite = leftBranch.BranchSprite;
            // _leftBranch.TeleportPosition = leftBranch.TeleportPosition;
            // _leftBranch.Type = BranchType.Left;
            // rightPrefab.TryGetComponent(out Branch rightBranch);
            // _rightBranch = rightBranch;
            // _rightBranch.IsCentered = rightBranch.IsCentered;
            // _rightBranch.BranchSprite = rightBranch.BranchSprite;
            // _rightBranch.TeleportPosition = rightBranch.TeleportPosition;
            // _rightBranch.Type = BranchType.Right;
            _pool.InitPool(leftPrefab, maxPoolSize, parent);
            GenerateInitPool();
            //player.transform.position = genBranchList[0].TeleportPosition.position;

        }

        private void Update()
        {
            if (!_positionChanged)
            {
                player.transform.position = _startPoint.position;
                if (player.transform.position.x > 0)
                {
                    player.transform.rotation = new Quaternion(0,-180,0, 0);
                }
                _positionChanged = true;
            }
        }

        void Generation(Random rnd, int size,bool firstGenerate = false, int prevInd = -1, int repeat = 0)
        {
            for (int i = 0; i < size; i++)
            {
                
                _pool.TryGetFromPool(out GameObject branchInstance);
                var ind = rnd.Next(0, 2);
                
                branchInstance.TryGetComponent(out Branch instance);
                if (prevInd == ind)
                    repeat++;
                else
                {
                    prevInd = ind;
                    repeat = 0;
                }
                if (repeat == restriction && ind == 0)
                {
                    ind = 1;
                    Debug.Log("Restriction works");
                }
                else if (repeat == restriction && ind == 1)
                {
                    ind = 0;
                    Debug.Log("Restriction works");
                }
                switch (ind)
                {

                    case 0:
                        //branchInstance.transform.rotation = new Quaternion(0,-180,0, 0);
                        instance.Type = BranchType.Left;
                        instance.BranchSprite = Resources.Load<Sprite>("left");
                        instance.IsCentered = false;
                        instance.TeleportPosition = branchInstance.transform.GetChild(0);
                        break;
                    case 1:
                        //branchInstance.transform.rotation = new Quaternion(0,0,0, 0);
                        instance.Type = BranchType.Right;
                        instance.BranchSprite = Resources.Load<Sprite>("right");
                        instance.IsCentered = false;
                        instance.TeleportPosition = branchInstance.transform.GetChild(1);
                        break;
                }


                _yTrans += 2.6f;
                branchInstance.transform.position = new Vector3(0, _yTrans , 0);
                
                if (i == 0 && firstGenerate)
                {
                    _startPoint = instance.TeleportPosition;
                    //_initialBranch = instance;
                    
                }

                if (i == 1 && firstGenerate)
                    instance.IsCentered = true;
                if(i == 0 && !firstGenerate)
                    instance.IsCentered = true;
                

                
                _genBranchList.Add(instance);
            }
            _teleportPlayer.SendNewBranches(_genBranchList);
        }

        void GenerateInitPool()
        {
            _genBranchList = new List<Branch>();
            _prevGenBranchList = new List<Branch>();
            Random rnd = new Random();
            
            Generation(rnd, maxPoolSize / 2, true);
            
            // for (int i = 0; i < maxPoolSize; i++)
            // {
            //     
            //     _pool.TryGetFromPool(out GameObject branchInstance);
            //     var ind = rnd.Next(0, 2);
            //     
            //     branchInstance.TryGetComponent(out Branch instance);
            //     if (prevInd == ind)
            //         repeat++;
            //     else
            //     {
            //         prevInd = ind;
            //         repeat = 0;
            //     }
            //     if (repeat == restriction && ind == 0)
            //     {
            //         ind = 1;
            //     }
            //     else if (repeat == restriction && ind == 1)
            //     {
            //         ind = 0;
            //     }
            //     switch (ind)
            //     {
            //
            //         case 0:
            //             instance.Type = _leftBranch.Type;
            //             instance.BranchSprite = _leftBranch.BranchSprite;
            //             instance.IsCentered = _leftBranch.IsCentered;
            //             break;
            //         case 1:
            //             branchInstance.transform.rotation = new Quaternion(0,0,0, 0);
            //             instance.Type = _rightBranch.Type;
            //             instance.BranchSprite = _rightBranch.BranchSprite;
            //             instance.IsCentered = _rightBranch.IsCentered;
            //              //TODO instance.TeleportPosition = branchInstance.transform.GetChild(1);
            //             break;
            //     }
            //
            //     yTrans += 3;
            //
            //     branchInstance.transform.position += new Vector3(0, yTrans, 0);
            //     
            //     if (i == 0 && firstGenerate)
            //     {
            //         _startPoint = instance.TeleportPosition;
            //         continue;
            //     }
            //     if (maxPoolSize / 2 == i || i == 0)
            //     {
            //         instance.IsCentered = true;
            //     }
            //
            //     
            //     _genBranchList.Add(instance);
            // }
            
            // _teleportPlayer.SendNewBranches(_genBranchList);
        }

        public async UniTask GenerateBranchesAsync()
        {
            await UniTask.Delay(2000);
            Random rnd = new Random();
            if (_pool.Branches.Count == 0)
            {
                Debug.Log(_prevGenBranchList.Count);
                Debug.Log("Halv of pool size is" + maxPoolSize / 2);
                for (int i = 0; i < maxPoolSize / 2; i++)
                {
                    Debug.Log(i);
                    
                    _pool.ReturnToPool(_prevGenBranchList[i].gameObject);
                    
                }
                
            }
            
            Debug.Log("Finished return");
            _prevGenBranchList.Clear();
            // if (_initialBranch != null)
            // {
            //     _prevGenBranchList.Add(_initialBranch);
            //     _initialBranch = null;
            // }
            foreach (var branch in _genBranchList)
            {
                _prevGenBranchList.Add(branch);
            }
            _genBranchList.Clear();
            Generation(rnd, maxPoolSize / 2);
            Debug.Log("generated and sent");
        }
    }
}
