from bs4 import BeautifulSoup
from requests import get
import urllib2
import sys
from StringIO import StringIO

def getContent(link):
    r = get(link).text
    soup = BeautifulSoup(r, 'lxml')
    f=""
    if len(soup.find_all("div", {"class": "story-body__inner"})) > 0:
        for htag in soup.find_all("div", {"class": "story-body__inner"}):
            for atag in htag.find_all('p'):
                acontent = atag.text.replace("."," . ").replace(","," , ").replace("?", " ? ").replace("\'s","").replace("u'","").replace("\n" ," ")
                f = f+ acontent
    elif len(soup.find_all("div", {"id": "story-page"})) > 0:
        for htag in soup.find_all("div", {"id": "story-page"}):
            for atag in htag.find_all('p'):
                acontent = atag.text.replace("."," . ").replace(","," , ").replace("?", " ? ").replace("\'s","").replace("u'","").replace("\n" ," ")
                f = f+ acontent
    elif len(soup.find_all("div", {"id": "story-body"})) > 0:
        for htag in soup.find_all("div", {"id": "story-body"}):
            for atag in htag.find_all('p'):
                acontent = atag.text.replace("."," . ").replace(","," , ").replace("?", " ? ").replace("\'s","").replace("u'","").replace("\n" ," ")
                f = f+ acontent
    else:
        for htag in soup.find_all("div"):
            for atag in htag.find_all('p'):
                acontent = atag.text.replace("."," . ").replace(","," , ").replace("?", " ? ").replace("\'s","").replace("u'","").replace("\n" ," ")
                f = f+ acontent
    print f

if __name__ == '__main__':
    getContent(sys.argv[1])

