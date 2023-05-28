Shader "Vertex Color"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
       Pass
        {
            lighting On
            ColorMaterial AmbientAndDiffuse
            SetTexture[_MainTex]
             {
              combine texture * primary DOUBLE
             }
       }
        
    }
    
}
