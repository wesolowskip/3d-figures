using GK_Projekt4.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4
{
    [Serializable]
    class SceneData
    {
        public SceneData(Figure[] figures, Camera[] cameras, Light[] lights, 
            bool backfaceCulling, bool zBuffer, bool lighting, bool perspectiveCorrection)
        {
            Figures = figures;
            Cameras = cameras;
            Lights = lights;
            BackfaceCulling = backfaceCulling;
            ZBuffer = zBuffer;
            Lighting = lighting;
            PerspectiveCorrection = perspectiveCorrection;
        }

        public Figure[] Figures { get; set; }
        public Camera[] Cameras { get; set; }
        public Light[] Lights { get; set; }
        public bool BackfaceCulling { get; set; }
        public bool ZBuffer { get; set; }
        public bool Lighting { get; set; }
        public bool PerspectiveCorrection { get; set; }
    }
}
