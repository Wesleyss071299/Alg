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
      int n = 0;
      int limit = 10;
      List<Orgao> orgaos = new List<Orgao>();
      List<Orgao> aux = new List<Orgao>();
      Orgao[] orgaosArray = new Orgao[limit * 15];
      double[] linearTimeArray = new double[limit];
      double[] binaryTimeArray = new double[limit];
      double[] bubbleTimeArray = new double[limit];

      string [] searchInput = new string[] {"10000", "20101", "20203", "20501", "20701", "22804", "25001", "25801", "25905", "26216"};

      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("chave-api-dados", "4bda51885581e6837f94ba4e6d34e710");
      
      while(n < limit)
      {
        
        var streamTask = await client.GetStreamAsync("http://api.portaldatransparencia.gov.br/api-de-dados/orgaos-siafi?pagina=" + Convert.ToString(n + 1));

        aux = await JsonSerializer.DeserializeAsync<List<Orgao>>(streamTask);
        orgaos = orgaos.Concat(aux).ToList();
        orgaosArray = orgaos.ToArray();

        // Binary Search 
        var watchBinary = System.Diagnostics.Stopwatch.StartNew();
        var resultBinary = binarySearch(orgaosArray, 0, orgaosArray.Length -1, searchInput[n]);
        watchBinary.Stop();
        var elapsedMsBinary = watchBinary.Elapsed.TotalMilliseconds;
        binaryTimeArray[n] = elapsedMsBinary;

        // Bubble Sort
        var watchBubble = System.Diagnostics.Stopwatch.StartNew();
        orgaosArray = bubbleSort(orgaosArray);
        watchBubble.Stop();
        var elapsedMsBubble = watchBubble.Elapsed.TotalMilliseconds;
        bubbleTimeArray[n] = elapsedMsBubble;

        // Linear Search
        var watchLinear = System.Diagnostics.Stopwatch.StartNew();
        var resultLinear = linearSearch(orgaosArray, searchInput[n]);
        watchLinear.Stop();
        var elapsedMsLinear = watchLinear.Elapsed.TotalMilliseconds;
        linearTimeArray[n] = elapsedMsLinear;



        n++;
      }


      return Ok(new { bubbleTimeArray, binaryTimeArray, linearTimeArray });

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
    public static Orgao[] bubbleSort(Orgao[] vetor)
    {
      int tamanho = vetor.Length;
      int comparacoes = 0;
      int trocas = 0;

      for (int i = tamanho - 1; i >= 1; i--)
      {
        for (int j = 0; j < i; j++)
        {
          comparacoes++;

          if (Convert.ToInt32(vetor[j].codigo) > Convert.ToInt32(vetor[j + 1].codigo))
          {
            Orgao aux = vetor[j];
            vetor[j] = vetor[j + 1];
            vetor[j + 1] = aux;
            trocas++;
          }
        }
      }

      return vetor;
    }

    static int binarySearch(Orgao[] arr, int l, int r, string cod)
    {
        if (r >= l) {
            int mid = l + (r - l) / 2;
  
            if (Convert.ToInt32(arr[mid].codigo) == Convert.ToInt32(cod))
                return mid;

            if (Convert.ToInt32(arr[mid].codigo) > Convert.ToInt32(cod))
                return binarySearch(arr, l, mid - 1, cod);

            return binarySearch(arr, mid + 1, r, cod);
        }
  
        return -1;
    }
    static int linearSearch(Orgao[] arr, string cod)
    {
      for (int i = 0; i < arr.Length; i ++)
      {
        if (Convert.ToInt32(arr[i].codigo) == Convert.ToInt32(cod)){
          return 1;
        }
      }
      return -1;
    }


  }
}
