﻿using System;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ParkingClient;

namespace ParkingClient
{
    
    public class UserManager
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
       
        
        public UserManager(string nome, string cognome, string telefono, string email, string pass)
        {
            Nome = nome;
            Cognome = cognome;
            Telefono = telefono;
            Email = email;
            Pass = pass;
        }
        public UserManager() { }
       
        

        #region public
        public void UserRegistration()
        {
            Console.WriteLine("\nInserisci i dati utente per procedere con la registrazione");
            try
            { 
              Nome =Validation.CheckInput("Inserisci il nome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
 
               Cognome= Validation.CheckInput("Inserisci il cognome: ", input =>
               {
                   return !string.IsNullOrWhiteSpace(input);
               });
               
                Telefono = Validation.CheckInput("Inserisci il nuovo numero di telefono: ", input =>
               {
                   return input.All(char.IsDigit) && input.Length >= 3; 
                  
               });

               Email = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
               {
                   return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
               }).ToLower();


                Pass = Validation.CheckInput("Inserisci la pass: ", input =>
               {
                   return !string.IsNullOrWhiteSpace(input);
               });

            }
            catch (Exception ex)
            {
                    Console.WriteLine($"Errore: {ex.Message}");
            }
        }
        
        public bool IsUtenteInserito()
        {
            return !string.IsNullOrWhiteSpace(Nome)
                && !string.IsNullOrWhiteSpace(Cognome)
                && !string.IsNullOrWhiteSpace(Telefono)
                && !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Pass);
        }
 
        public void EditUser()
        {
            try
            {
                if (Nome == null)
                {
                    Console.WriteLine("Nessun utente presente. Inserisci una persona prima di effettuare una modifica.");
                    return;
                }
                Console.WriteLine("\nModifica i dati inseriti precedentemente:");
               
                Console.Write("Nome attuale: {0}", Nome);
                string nuovoNome = Validation.CheckInput("Inserisci il nuovo nome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
                Console.Write("Cognome attuale: {0}", Cognome);
                string nuovoCognome = Validation.CheckInput("Inserisci il nuovo cognome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                string nuovaPassword = Validation.CheckInput("Inserisci la nuova pass: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                string nuovoNumeroTelefono = Validation.CheckInput("Inserisci il nuovo numero di telefono: ", input =>
                {
                    return input.All(char.IsDigit) && input.Length >= 3;
                });

                Console.Write("Indirizzo e-mail attuale {0}", Email);
                string nuovaEmail = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
                }).ToLower();
                // Aggiornamento dei dati della persona
                Nome = nuovoNome;
                Cognome = nuovoCognome;
                Pass = nuovaPassword;
                Telefono = nuovoNumeroTelefono;
                Email = nuovaEmail;

                Console.WriteLine("\nDati del utente modificati con successo.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }

        public bool DoLogin()
        {
            Console.WriteLine("Benvenuto! Effettua il login.");
            bool accessoConcesso = false;
            do
            {
                Email = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
                }).ToLower();

                Pass = Validation.CheckInput("Inserisci la password: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
                //get vedere meglio che fann
                accessoConcesso = CheckLogin(Email, Pass).GetAwaiter().GetResult();

                if (accessoConcesso)
                {
                    Console.WriteLine("Accesso consentito. Benvenuto!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Accesso negato. Riprova.");
                }
            } while (!accessoConcesso);
            return false;
        }


        #endregion

        #region private 
        private async Task<bool> CheckLogin(string email, string password)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                var requestData = new { Email = email, Pass = password };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:18080/login", content);
                //response.StatusCode.ToString().Contains("200")
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
            {
                Console.WriteLine("Si è verificato un errore durante la verifica dell'accesso: " + ex.Message);
                return false;
            }
        }
        #endregion

    }
}







