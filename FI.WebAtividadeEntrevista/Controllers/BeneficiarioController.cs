using WebAtividadeEntrevista.Models;
using System.Web.Mvc;
using System.Linq;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FI.WebAtividadeEntrevista.Models.Services;
using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {

        [HttpPost]
        public JsonResult SalvarBeneficiario(BeneficiarioModel model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            try
            {
                //Deixa apenas os numeros para serem cadastrados no DB
                string cpfSomenteNumeros = Regex.Replace(model.CPF, @"\D", "");

                CPFValidator CPFValidator = new CPFValidator();

                if (!CPFValidator.ValidaCPF(cpfSomenteNumeros))
                {
                    Response.StatusCode = 400;
                    return Json("CPF inválido");
                }

                BoBeneficiario bo = new BoBeneficiario();

                model.Id = bo.Incluir(new Beneficiario()
                {
                    Nome = model.Nome,
                    CPF = cpfSomenteNumeros,
                    IdCliente = model.IdCliente,
                });

                return Json("Cadastro efetuado com sucesso");
            }
            catch (Exception ex) when (ex.Message.Contains("O CPF informado já está cadastrado"))
            {
                Response.StatusCode = 400;
                return Json("O CPF informado já está cadastrado.");
            }
            catch (SqlException ex)
            {
                Response.StatusCode = 500;
                return Json("Erro de banco de dados: ", ex.Message);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json("Erro inesperado: ", ex.Message);
            }
        }

        [HttpPost]
        public JsonResult ExcluirBeneficiario(BeneficiarioModel model)
        {
            return Json(new { sucesso = true });
        }
    }
}