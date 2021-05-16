$(document).ready(function () {
    fetchRawGraphData();
});

let graphData;

const fetchRawGraphData = () => {
    $.ajax({
        type: 'GET',
        url: 'http://54.85.250.76:3000/api/history',
        //url: 'http://localhost:3000/api/history',
    }).then(result => {
        graphData = formatGraphData(result);
        //console.log(graphData);
        renderGraph();
    });
};

const formatGraphData = (dati) => {
    let formattedGraphData = [];

    for (const x of dati) {
        let data = formattedGraphData[graphDateFormat(x.data_consegna)];
        let pezziTotali = x.quantita_prodotta;
        let pezziScartati = x.quantita_scarto_difettoso + x.quantita_scarto_pieno;

        if (data == undefined) {
            data = {
                "pezzi_totali": pezziTotali,
                "pezzi_scartati": pezziScartati
            }
        } else {
            data = {
                "pezzi_totali": pezziTotali + data.pezzi_totali,
                "pezzi_scartati": pezziScartati + data.pezzi_scartati
            }
        }
        formattedGraphData[graphDateFormat(x.data_consegna)] = data;
    }

    let data_x = [];
    let pezziTotaliGiornalieri = [];
    let pezziScartatiGiornalieri = [];

    Object.keys(formattedGraphData).forEach(function (key) {
        data_x.push(key);
        pezziTotaliGiornalieri.push(formattedGraphData[key].pezzi_totali);
        pezziScartatiGiornalieri.push(formattedGraphData[key].pezzi_scartati);
    });

    return [data_x, pezziTotaliGiornalieri, pezziScartatiGiornalieri];
}

const graphDateFormat = (rawDate) => {
    let newDate = new Date(rawDate);
    let formattedDate = `${newDate.getFullYear()}-${("0" + (newDate.getMonth() + 1)).slice(-2)}-${("0" + newDate.getDate()).slice(-2)}`
    return formattedDate;
}

const renderGraph = () => {
    var optionsLine = {
        chart: {
            width: "95%",
            foreColor: "#f8f9fa",
            background: "#0e0831",
            toolbar: {
                show: true
            },
            height: "200%",
            type: 'line',
            zoom: {
                enabled: false
            },
            dropShadow: {
                enabled: true,
                top: 3,
                left: 2,
                blur: 4,
                opacity: 1,
            }
        },
        tooltip: {
            theme: "dark"
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        colors: ["#00F790", '#F7AA00'],
        series: [{
            name: "pezzi prodotti",
            data: graphData[1]
        },
        {
            name: "pezzi scartati",
            data: graphData[2]
        }
        ],
        markers: {
            size: 6,
            strokeWidth: 0,
            hover: {
                size: 9
            }
        },
        grid: {
            show: true,
            padding: {
                bottom: 0
            }
        },
        labels: graphData[0],
        xaxis: {
            tooltip: {
                enabled: false
            }
        },
        legend: {
            position: 'bottom',
            horizontalAlign: 'center'
        },
        responsive: [
            {
                breakpoint: 1000,
                options: {
                }
            }
        ]
    }

    var chartLine = new ApexCharts(document.querySelector('#graph'), optionsLine);
    chartLine.render();
}