﻿using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using FI.WebAtividadeEntrevista.Models.Services;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
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
                // Deixa apenas os numeros para serem cadastrados no DB
                string cpfSomenteNumeros = Regex.Replace(model.CPF, @"\D", "");

                CPFValidator CPFValidator = new CPFValidator();

                if (!CPFValidator.ValidaCPF(cpfSomenteNumeros))
                {
                    Response.StatusCode = 400;
                    return Json("CPF inválido");
                }

                BoCliente bo = new BoCliente();

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = cpfSomenteNumeros,
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
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            try
            {
                // Deixa apenas os numeros para serem cadastrados no DB
                string cpfSomenteNumeros = Regex.Replace(model.CPF, @"\D", "");

                CPFValidator CPFValidator = new CPFValidator();

                if (!CPFValidator.ValidaCPF(cpfSomenteNumeros))
                {
                    Response.StatusCode = 400;
                    return Json("CPF inválido");
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = cpfSomenteNumeros,
                });



                return Json("Cadastro alterado com sucesso");

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

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}