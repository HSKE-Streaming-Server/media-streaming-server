GITHUB_USER="niklasstich"
GITHUB_TOKEN="d47a66ffde5feeacebb860c933d78f327120de5e"

sudo apt update
sudo apt install -y curl jq unzip
curl -sS -u "$GITHUB_USER":"$GITHUB_TOKEN" https://api.github.com/repos/hske-streaming-server/media-streaming-client/actions/artifacts > artifacts.json
FAKEDL=$(jq '.artifacts | max_by(.id) | .archive_download_url' artifacts.json | tr -d '"')
curl -sS -u "$GITHUB_USER":"$GITHUB_TOKEN" -L "$FAKEDL" -o dist.zip
rm ./webroot -rf
mkdir webroot
unzip dist.zip -d webroot
