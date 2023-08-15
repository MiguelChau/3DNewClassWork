using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cloth;

namespace Cloth
{
    public class ClothChange : MonoBehaviour
    {

        public SkinnedMeshRenderer mesh;

        public Texture2D texture;
        public string shaderName = "_EmissionMap";

        private Texture2D _defaultTexture;
        private void Awake()
        {
            _defaultTexture = (Texture2D)mesh.materials[0].GetTexture(shaderName);
        }

        [NaughtyAttributes.Button]
        private void ChangeTexture()
        {
            mesh.materials[0].SetTexture(shaderName, texture);
        }

        public void ChangeTexture(ClothSetup setup)
        {
            mesh.materials[0].SetTexture(shaderName, setup.texture2D);
        }

        public void ResetTexture()
        {
            mesh.materials[0].SetTexture(shaderName, _defaultTexture);
        }
    }
}

