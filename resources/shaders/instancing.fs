#version 330 core
out vec4 FragColor;

//in vec2 TexCoords;

in VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
} fs_in;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;
uniform Light light;
uniform sampler2D texture_diffuse1;
uniform sampler2D floorTexture;
uniform vec3 lightPos;
uniform vec3 viewPos;
uniform bool blinn;


void main()
{
       // ambient
           vec3 ambient = light.ambient * texture(material.diffuse, fs_in.TexCoords).rgb;

           // diffuse
           vec3 norm = normalize(fs_in.Normal);
           vec3 lightDir = normalize(light.position - fs_in.FragPos);
           float diff = max(dot(norm, lightDir), 0.0);
           vec3 diffuse = light.diffuse * diff * texture(material.diffuse, fs_in.TexCoords).rgb;

   // vec3 color = texture(texture_diffuse1, fs_in.TexCoords).rgb;
        // ambient
    //    vec3 ambient = 0.4 * color;
        // diffuse
    //    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    //    vec3 normal = normalize(fs_in.Normal);
    //    float diff = max(dot(lightDir, normal), 0.0);
    //    vec3 diffuse = diff * color;
    //    //specular
    //    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    //    vec3 reflectDir = reflect(-lightDir, normal);
    //       float spec = 0.0;

           vec3 viewDir = normalize(viewPos - fs_in.FragPos);
              vec3 reflectDir = reflect(-lightDir, norm);
              float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);


           if(blinn)
           {
               vec3 halfwayDir = normalize(lightDir + viewDir);
               spec = pow(max(dot(norm, halfwayDir), 0.0), 32.0);
           }
           else
           {
               vec3 reflectDir = reflect(-lightDir, norm);
               spec = pow(max(dot(viewDir, reflectDir), 0.0), 8.0);
           }
           //vec3 specular = vec3(0.3) * spec; // assuming bright white light color
              vec3 specular = light.specular * spec * texture(material.specular, fs_in.TexCoords).rgb;

    FragColor = vec4(ambient + diffuse + specular, 1.0);
}
