const prodottiSchema = mongoose.Schema({
    id_commessa : Number,
    articolo : String,
    quantita_prevista : Number,
    data_consegna : Date,
    quantita_prodotta : Number,
    quantita_scarto : Number,
    
    toJSON: {virtuals: true},
    toObject: {virtuals: true}
});

TodoSchema.virtual('completed')
    .get(function() {
        return this.quantita_prevista > this.quantita_prodotta;
    });

module.exports = mongoose.model('prodotti', prodottiSchema);
