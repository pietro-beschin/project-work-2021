# coding=utf-8
from telegram.ext import Updater, CommandHandler, MessageHandler, Filters
import requests


# Token:
TOKEN = "1881817777:AAEmSCzo2VIN0EJj0hTi19VAeNC0zo7Gubg"


def convert_json_commessa_and_send(rjson, update):
    codice = rjson['codice_commessa']
    articolo = rjson['articolo']
    quantitaprevista = str(rjson['quantita_prevista'])
    quantitaprodotta = str(rjson['quantita_prodotta'])
    quantitascartata = str(rjson['quantita_scarto_pieno'] + rjson['quantita_scarto_difettoso'])
    dataconsegna = modifica_data(rjson['data_consegna'])
    dataesecuzione = modifica_data(rjson['data_esecuzione'])
    stato = rjson['stato']

    rtext = 'codice commessa: ' + codice + '\n' + 'articolo: ' + articolo + '\n' + 'quantità prevista: ' + quantitaprevista + '\n' + 'quantità prodotta: ' + quantitaprodotta + '\n' + 'quantità scartata: ' + quantitascartata + '\n' + 'data esecuzione: ' + dataesecuzione + '\n' + 'data consegna: ' + dataconsegna + '\n' + 'stato: ' + stato
    update.message.reply_text(rtext)


def modifica_data(date):
    newdate = date.split('T')[0]
    newtime = date.split('T')[1]

    strdate = newdate.split('-')[2]+'/'+newdate.split('-')[1]+'/'+newdate.split('-')[0]
    strtime = newtime.split(':')[0]+':'+newtime.split(':')[1]
    return strdate+' '+strtime


# Metodo per visualizzare l'ultima commessa
def view_commessa(update, context):
    r = requests.get('http://54.85.250.76:3000/api/lastCommessa')
    rjson = r.json()

    convert_json_commessa_and_send(rjson, update)


# Metodo per visualizzare lo stato macchina
def view_stato_macchina(update, context):
    r = requests.get('http://54.85.250.76:3000/api/status')
    rjson = r.json()

    stato = rjson['stato']
    velocita = str(rjson['velocita'])
    allarme = rjson['allarme']

    rtext = 'stato: '+stato+'\n'+'pezzi/ora: '+velocita+'\n'+'allarme: '+allarme
    update.message.reply_text(rtext)


# Metodo per visualizzare lo storico delle commesse
def view_storico_commesse(update, context):
    r = requests.get('http://54.85.250.76:3000/api/history')
    rjson = r.json()

    for x in rjson:
        convert_json_commessa_and_send(x, update)


def view_info(update, context):
    r1 = requests.get('http://54.85.250.76:3000/api/status')
    r1json = r1.json()

    velocita = str(r1json['velocita'])

    r2 = requests.get('http://54.85.250.76:3000/api/lastCommessa')
    r2json = r2.json()

    stato = r2json['stato']
    quantitaprevista = r2json['quantita_prevista']
    quantitaprodotta = r2json['quantita_prodotta']

    percentualeproduzione = str(100 * quantitaprodotta / quantitaprevista)

    rtext = 'stato: '+stato+'\n'+'avanzamento: '+percentualeproduzione+'%'+'\n'+'pezzi/ora: '+velocita

    update.message.reply_text(rtext)


def main():
    upd = Updater(TOKEN, use_context=True)
    disp = upd.dispatcher

    disp.add_handler(CommandHandler("info", view_info))
    disp.add_handler(CommandHandler("commessa", view_commessa))
    disp.add_handler(CommandHandler("macchina", view_stato_macchina))
    disp.add_handler(CommandHandler("storico", view_storico_commesse))

    upd.start_polling()

    upd.idle()


if __name__ == '__main__':
    main()
