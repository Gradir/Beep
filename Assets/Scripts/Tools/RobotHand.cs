using UnityEngine;
using System.Collections;

namespace GGJ2020.Tools
{
    public class RobotHand : MonoBehaviour
    {
        [SerializeField] private Transform toolHolder;
        public Tool currentTool;

        #region Equipping tools
        public void EquipTool(Tool toolToEquip)
        {
            UnequipTool();
            currentTool = Instantiate<Tool>(toolToEquip, toolHolder.position, toolHolder.rotation, toolHolder);
        }

        public void UnequipTool()
        {
            if (currentTool != null)
            {
                Destroy(currentTool.gameObject);
                currentTool = null;
            }
        }
        #endregion
    }
}