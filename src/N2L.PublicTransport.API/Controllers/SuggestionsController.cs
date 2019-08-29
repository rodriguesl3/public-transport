using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using N2L.PublicTransport.Domain.Entities;

namespace N2L.PublicTransport.API.Controllers
{
    [Route("api/suggestions")]
    [ApiController]
    public class SuggestionsController : Controller
    {
        public IActionResult Index()
        {
            var suggestionsList = new List<Suggestions>
            {
                new Suggestions("https://www.aworldtotravel.com/wp-content/uploads/2017/07/baixa-and-castle-things-lisbon-is-famous-for-a-world-to-travel.jpg",
                "Castelo de São Jorge",
                "A journey here provides you an experience like no other. Be amazed at the beauty of the medieval architecture and view you get from the castle, which sits prettily on top of Alfama’s hill.",
                latitude: 38.7139092,
                longitude: -9.1356595
                ),
                new Suggestions("https://www.100resilientcities.org/wp-content/uploads/2017/06/Lisbon-hero-crop.jpg",
                "Castelo de São Jorge",
                "A journey here provides you an experience like no other. Be amazed at the beauty of the medieval architecture and view you get from the castle, which sits prettily on top of Alfama’s hill.",
                latitude: 38.7139092,
                longitude: -9.1356595
                ),
                new Suggestions("https://i1.wp.com/www.aworldtotravel.com/wp-content/uploads/2017/07/portuguese-custard-tart-things-lisbon-is-famous-for-a-world-to-travel.jpg?zoom=1.25&w=515&h=281&ssl=1",
                "PASTEIS DE BELEM",
                "Food is huge here in Lisbon, and it doesn’t get any better than the famed Portuguese egg tarts, specifically those served here in Pasteis de Belem.",
                latitude: 38.6978909,
                longitude: -9.2088872
                ),



                new Suggestions("http://www.liven.pt/wp-content/uploads/2015/05/praca-do-comercio.jpg?7a17fc",
                "Praça do Comércio",
                "A tour da cidade começa na Praça do Comércio, a grande praça neoclássica que ancora a cidade ao rio Tejo. Andem pela margem do rio para sentirem a cidade. Também é muito bonita vista do rio, num passeio de barco. A maior praça de Lisboa e também uma das mais emblemáticas, símbolo da cidade e da sua reconstrução após o grande terramoto de 1755.",
                latitude:38.7075195,
                longitude: -9.140789
                ),
                new Suggestions("http://www.liven.pt/wp-content/uploads/2015/05/miradouro-de-sao-pedro-de-alcantara.jpg?7a17fc",
                "Miradouros",
                "Lisboa é uma cidade cheia de paisagens. Pressionada contra o rio Tejo, as sete colinas combinam-se para criar uma cidade de vistas deslumbrantes. No topo das colinas existem miradouros, uma série de praças pedonais que oferecem uma magnífica vista da cidade.",
                latitude: 38.7115991,
                longitude: -9.1347101
                ),
                new Suggestions("http://www.liven.pt/wp-content/uploads/2015/05/Percursos-electricos_elevadores-Elevador_St_Justa_Unf_44.jpg?7a17fc",
                "Elevador de Santa Justa",
                "Inesperado e icónico, este elevador de ferro é a atracção mais surpreendente de Lisboa, uma espécie de mistura entre a Torre Eiffel e uma torre de controlo de tráfego aéreo. Tem uma vista invejável sobre esta parte antiga de Lisboa, para além de ser um privilégio viajar neste elevador com mais de cem anos que foi desenhado por Ponsard, um discípulo do grande mestre das obras em ferro, Gustave Eiffel.  Faz a ligação entre a Baixa e o Bairro Alto.",
                latitude: 38.7121023,
                longitude: -9.1416208
                ),
                new Suggestions("http://www.liven.pt/wp-content/uploads/2015/05/torre-de-belem-lisboa.jpg?7a17fc",
                "Torre de Belém",
                "É um dos pontos altos de Lisboa e um dos monumentos mais pitorescos da Europa. Para além de as abóbadas trabalhadas em pedra constituírem uma obra de engenharia admirável, a riqueza dos elementos decorativos ligados a aspetos marítimos e às viagens dos navegadores é fascinante.  Além de tudo isto, uma das atracções da torre é um rinoceronte esculpido – o primeiro na Europa! É património Mundial.",
                latitude: 38.6915837,
                longitude: -9.2181606
                ),
                new Suggestions("http://www.liven.pt/wp-content/uploads/2015/05/mosteiro-dos-jeronimos-lisboa.jpg?7a17fc",
                "Mosteiro dos Jerónimos",
                "Uma caixinha de doces arquitectural, o Mosteiro dos Jerónimos é um exemplo fantástico do estilo Manuelino e a principal atracção turística de Lisboa. Este monumento deslumbrante, classificado como Património Mundial pela UNESCO, não deixa ninguém indiferente. Retratando a riqueza da Coroa Portuguesa, bem como a capacidade criativa de D. Manuel I e do arquitecto Diogo de Boitaca, este monumento de 300 metros de comprimento é um dos exemplos mais impressionantes de arquitectura religiosa de todo o mundo.",
                latitude: 38.6978909,
                longitude: -9.2088872
                ),
                new Suggestions("https://www.100resilientcities.org/wp-content/uploads/2017/06/Lisbon-hero-crop.jpg",
                "Castelo de São Jorge",
                "A journey here provides you an experience like no other. Be amazed at the beauty of the medieval architecture and view you get from the castle, which sits prettily on top of Alfama’s hill.",
                latitude: 38.7139092,
                longitude: -9.1356595
                ),new Suggestions("https://www.100resilientcities.org/wp-content/uploads/2017/06/Lisbon-hero-crop.jpg",
                "Castelo de São Jorge",
                "A journey here provides you an experience like no other. Be amazed at the beauty of the medieval architecture and view you get from the castle, which sits prettily on top of Alfama’s hill.",
                latitude: 38.7139092,
                longitude: -9.1356595
                ),


            };

            return Ok(suggestionsList);
        }
    }

}