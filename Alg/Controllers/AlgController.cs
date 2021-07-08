using Alg.Comparable;
using Alg.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlgController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Orgao>> Get()
        {
            List<Orgao> orgaos = null;
            var client = new HttpClient();
            var cod = "01000";

            client.DefaultRequestHeaders.Add("chave-api-dados", "4bda51885581e6837f94ba4e6d34e710");


            var streamTask = client.GetStreamAsync("http://api.portaldatransparencia.gov.br/api-de-dados/orgaos-siafi?pagina=1");

            orgaos = await JsonSerializer.DeserializeAsync<List<Orgao>>(await streamTask);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            foreach (Orgao orgao in orgaos)
            {
                if (orgao.codigo.ToLower() == cod.ToLower())
                {
                    watch.Stop();
                    var elapsedMs = watch;
                    return Ok(new { time = elapsedMs});
                }
            }

            return NotFound();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Orgao>>> GetALL()
        {
            List<Orgao> orgaos = null ;



            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("chave-api-dados", "4bda51885581e6837f94ba4e6d34e710");



            var streamTask = client.GetStreamAsync("http://api.portaldatransparencia.gov.br/api-de-dados/orgaos-siafi?pagina=1");

            orgaos = await JsonSerializer.DeserializeAsync<List<Orgao>>(await streamTask);

            return orgaos;
        }

        [HttpGet]
        [Route("Binary")]
        public async Task<ActionResult<Orgao>> GetBinary()
        {
            List<Orgao> orgaos = null;
            var teste = new Orgao();
            teste.codigo = "01000";
            teste.descricao = "Câmara dos Deputados - Unidades com vínculo direto";


            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("chave-api-dados", "4bda51885581e6837f94ba4e6d34e710");



            var streamTask = client.GetStreamAsync("http://api.portaldatransparencia.gov.br/api-de-dados/orgaos-siafi?pagina=1");

            orgaos = await JsonSerializer.DeserializeAsync<List<Orgao>>(await streamTask);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            var index = orgaos.BinarySearch(teste, new OrgaoComparer());
            watch.Stop();
            if (index < 0)
            {
                return NotFound();
            }
           
            
            var elapsedMs = watch;
   

            return Ok(new { time = elapsedMs});
        }
    }
}
