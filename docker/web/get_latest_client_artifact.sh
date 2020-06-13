GITHUB_USER=""
GITHUB_TOKEN=""

curl -sS -u "$GITHUB_USER":"$GITHUB_TOKEN" https://api.github.com/repos/hske-streaming-server/media-streaming-client/actions/artifacts > artifacts.json
FAKEDL=$(jq '.artifacts | max_by(.id) | .archive_download_url' artifacts.json | tr -d '"')
curl -sS -u "$GITHUB_USER":"$GITHUB_TOKEN" -L "$FAKEDL" -o dist.zip