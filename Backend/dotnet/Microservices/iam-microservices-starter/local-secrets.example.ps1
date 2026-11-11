# Copy values into your local shell. Never commit populated secrets.
$env:Clients__Billing__Secret = "REPLACE_WITH_RANDOM_BILLING_SECRET"
$env:Clients__Payment__Secret = "REPLACE_WITH_RANDOM_PAYMENT_SECRET"
$env:ServiceAuth__ClientSecret = "REPLACE_WITH_BILLING_SECRET" # switch to Payment secret when running Payment
$env:BootstrapAdmin__Email = "admin@example.test"
$env:BootstrapAdmin__Password = "REPLACE_WITH_STRONG_PASSWORD"
