#!/bin/bash
cp -vr docs/_site/* .
git add .
git commit -m "Update website DocFX"
git push origin gh-pages