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
            double[] linearArray= new double[50];
            double[] binaryArray= new double[50];
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("chave-api-dados", "4bda51885581e6837f94ba4e6d34e710");


            var streamTask = client.GetStreamAsync("http://api.portaldatransparencia.gov.br/api-de-dados/orgaos-siafi?pagina=1");

            orgaos = await JsonSerializer.DeserializeAsync<List<Orgao>>(await streamTask);
            
            var teste = new Orgao();
            teste.codigo = "11000";
            teste.descricao = "Superior Tribunal de Justiça - Unidades com vínculo direto";

            for (var i = 0; i < binaryArray.Length; i++)
            {
              var watch2 = System.Diagnostics.Stopwatch.StartNew();
              
              var index = orgaos.BinarySearch(teste, new OrgaoComparer());
              watch2.Stop();
              if (index < 0)
              {
                  return NotFound();
              }
              var elapsedMs = watch2.Elapsed.TotalMilliseconds;
              binaryArray[i] = elapsedMs;
            }

            for (var i = 0; i < linearArray.Length ; i++)
            {
              var watch = System.Diagnostics.Stopwatch.StartNew();
              foreach (Orgao orgao in orgaos)
              {
                  if (orgao.codigo.ToLower() == teste.codigo.ToLower() && orgao.descricao.ToLower() == teste.descricao.ToLower())
                  {
                      watch.Stop();
                      var elapsedMs = watch.Elapsed.TotalMilliseconds;
                      linearArray[i] = elapsedMs;
                  }
              }
            }
            
            

            return Ok(new { linear = linearArray, binary = binaryArray });
            
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<Orgao>> GetAll()
        {
            List<Orgao> orgaos = null;

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("chave-api-dados", "4bda51885581e6837f94ba4e6d34e710");


            var streamTask = client.GetStreamAsync("http://api.portaldatransparencia.gov.br/api-de-dados/orgaos-siafi?pagina=1");

            orgaos = await JsonSerializer.DeserializeAsync<List<Orgao>>(await streamTask);
            

            

            return Ok(new { orgaos });
            
        }
    }
}
