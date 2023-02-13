/***
 *
 * @Author: Roman
 * @Created on: 11/02/23
 *
 ***/

using System;
using UnityEditor;
using UnityEngine;

namespace _3_Scripts.Editor.Optimization
{
    public class AssetsPostProcessor : AssetPostprocessor
    {
        #region ## Fields ##



        #endregion

        #region ## Properties ##



        #endregion

        #region ## Core ##

        private void OnPreprocessAnimation()
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
        }
        
        private void OnPreprocessAudio()
        {
            AudioImporter audioImporter = assetImporter as AudioImporter;
            
            if (audioImporter)
            {
                audioImporter.forceToMono = false;
                audioImporter.loadInBackground = true;
                audioImporter.preloadAudioData = true;
                audioImporter.defaultSampleSettings = new AudioImporterSampleSettings
                {
                    loadType = AudioClipLoadType.CompressedInMemory,
                    compressionFormat = AudioCompressionFormat.Vorbis,
                    quality = 0.5f
                };
            }
        }

        private void OnPreprocessModel()
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
 
            if (modelImporter) 
            {
                modelImporter.meshCompression = ModelImporterMeshCompression.High;
            }
        }
        
        private void OnPreprocessTexture()
        {
            TextureImporter textureImporter = assetImporter as TextureImporter;
            
            if (textureImporter)
            {
                textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
            }
        }

        #endregion
    }
}