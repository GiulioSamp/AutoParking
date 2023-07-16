import requests
import json
import datetime
import locale
#forse meglio nome ServerCommunication?
class Endpoint:
    def __init__(self, admin_id):
        self.admin_id = admin_id

    def data_to_server(self, endpoint, data):
        url = "http://localhost:18080" + endpoint
        payload = {"admin_id": self.admin_id, "data": data}

        try:
            response = requests.post(url, data=payload)
            response.raise_for_status()  # Genera un'eccezione se la richiesta non ha successo
            print("Richiesta inviata al server con successo!")
            return response.text  # Restituisci la risposta come stringa di testo
        except requests.exceptions.RequestException as e:
            print("Errore nell'invio della comunicazione al server:", e)
#uc3
    def get_occupied_spots_all_floors(self):
        endpoint = "/spots"
        response = self.data_to_server(endpoint, None)

        if response is not None:
            try:
                data = json.loads(response)  # la risposta JSON
                result = data["result"]  # estraggo il campo {"result":" dalla risposta
                result = result.strip()  #  eventuali spazi iniziali e finali lstrip()solo iniz
                result = result.replace('\\n', '\n')  # Sostituisci "\\n" con "\n" per andare a capo nella stampa
                print("Visualizzazione numero di posti occupati su numero di posti liberi")
                print(result)
                # return result se mi serve val ritorn
            except json.JSONDecodeError as e:
                print("Errore nella decodifica della risposta JSON:", e)
            except KeyError as e:
                print("La risposta non contiene il campo 'result':", e)
            except Exception as e:
                print("Errore nella gestione della risposta del server:", e)
                # return None
        else:
            print("Errore nella richiesta dei posti occupati.")
            # return None
#uc4
    def get_occupied_spots_forf(self):
        endpoint = "/spots"
        response = self.data_to_server(endpoint, None)

        if response is not None:
            try:
                data = json.loads(response)  # la risposta JSON
                result = data["result"]  # estraggo il campo "result" dalla risposta
                result = result.strip()  #  eventuali spazi iniziali e finali lstrip()solo iniz
                result = result.replace('\\n', '\n')  # Sostitu"\\n" con "\n" per andare a capo nella stampa 
                # Chiedi all'utente di inserire il numero del piano
                while True:
                    try:
                        piano_numero = int(input("Inserisci il numero del piano da 1 a 10: "))
                        if piano_numero < 1 or piano_numero > 10:
                            raise ValueError("Numero piano non valido.")
                        break
                    except ValueError:
                        print("Errore: Inserisci un numero di piano valido da 1 a 10.")

                piano_stringa = f"Piano: {piano_numero},"
                piano_inizio = result.index(piano_stringa)
                piano_fine = result.index('\n', piano_inizio)
                piano_risultato = result[piano_inizio:piano_fine]
                print(piano_risultato)
                #else:
                #print("Il piano specificato non � presente nella risposta.")

            except json.JSONDecodeError as e:
                print("Errore nella decodifica della risposta JSON:", e)
            except KeyError as e:
                print("La risposta non contiene il campo 'result':", e)
            except Exception as e:
                print("Errore nella gestione della risposta del server:", e)
                # return None
 

    def get_gain(self):
        endpoint = "/gain"
        response = self.data_to_server(endpoint, None)
        if response is not None:
            try:
                data = json.loads(response)  # Decodifica la risposta JSON
                if isinstance(data, list) and len(data) > 0:
                    #inserire l'anno, il mese e il giorno
                    year = int(input("Inserisci l'anno: "))
                    month = int(input("Inserisci il mese: "))
                    day = int(input("Inserisci il giorno: "))
                    user_date = datetime.date(year, month, day)
                    locale.setlocale(locale.LC_ALL, "") #formattazione locale ,
                    print("Importi:")
                    for profit in data:
                        if "Data" in profit:
                            # la parte "aaaa-mm-gg" dal campo "Data" dell'elemento
                            data_part = profit["Data"].split(" ")[0]
                            profit_date = datetime.datetime.strptime(data_part, "%Y-%m-%d").date()
                            if profit_date == user_date:
                                if "Importo" in profit:
                                    importo = profit["Importo"]                                    
                                    importo_formattato = locale.format_string("%.2f", float(importo), grouping=True)
                                    print(importo_formattato)
                                    
                else:
                    print("La risposta non contiene dati di guadagno.")
            except json.JSONDecodeError as e:
                print("Errore nella decodifica della risposta JSON:", e)
            except ValueError as e:
                print("Errore: Input non valido.", e)
            except Exception as e:
                print("Errore nella gestione della risposta del server:", e)
        else:
            print("Errore nella richiesta dei posti occupati.")
            
           






# Utilizzo dell'oggetto Endpoint
#endpoint_obj = Endpoint(admin_id="Admin12345")
#endpoint_obj.data_to_server("/api/endpoint1", {"key": "value"})
    


