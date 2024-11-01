using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlueRiver.Items
{
    public enum TreeItemType
    {
        None,
        Tree1,
        Tree2,
        Tree3,
        Tree4,
        Tree5,
    }


    public class TreeItemIcon : MonoSingleton<TreeItemIcon>
    {
        public List<Trees> trees = new List<Trees>();

        public Tree SearchTree(TreeItemType type)
        {
            var icon = trees.FirstOrDefault(trees => trees.treeType == type) ?? trees.FirstOrDefault(trees => trees.treeType == TreeItemType.None);

            return icon.icon;
        }

        [Serializable]
        public class Trees
        {
            public TreeItemType treeType;
            public Tree icon;
        }

    }
}