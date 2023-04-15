Shader "Unlit/PlayerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Volume("volume",Range(0,1)) = 0
        _Color("color",color) = (0,1,0)
        _Border("border",float) = 0
    }

    SubShader
    {

        Cull Off
        AlphaToMask On

         Tags
        {
            "Queue" = "AlphaTest"
            "RenderType" = "AlphaTest"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
          
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Volume;
            float4 _Color;
            float _Border;
           
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
               //_Border = 0.3;
               fixed4 color = tex2D(_MainTex, i.uv);
               color *= 1 - (step(_Border,i.uv.y));

               fixed borderSize = 0.05;
               fixed4 borderLine = (step(_Border,i.uv.y + borderSize) * 
               step(i.uv.y + borderSize,_Border + borderSize)) * _Color;

               borderLine.a = color.a;
               color += borderLine;

               if (color.a <= 0)
               {
                   discard;    //画素を塗りつぶさない。ZBufferの更新も行わない
               }


               //　グレースケール化
               //fixed gray = dot(color.rag,fixed3(0.299,0.587,0.114));
               //fixed4 targetColor = _Color * gray;

               //fixed4 viewColor = lerp(color,targetColor,_Volume);
               //return fixed4(viewColor.r,viewColor.g,viewColor.b,tex2D(_MainTex, i.uv).a);

               return color;
            }
            ENDCG
        }
    }
}
