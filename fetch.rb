require 'csv'
require 'bitbucket_rest_api'

$logins = Hash[ CSV.read('bitbucket-logins.csv')[1..-1].select do |fname, lname, id|
                  fname and lname and id
                end.map do |fname, lname, id|
                  [ id.strip, "#{lname.strip.downcase.gsub(/\s/, '-')}-#{fname.strip.downcase}" ]
                end ]


bb = BitBucket.new basic_auth: 'fvogels:4qK7DfDZW742NvLY'

forks_of_picross = bb.repos.list.select do |repo|
  fork_of = repo.fork_of

  fork_of and fork_of.owner == 'fvogels' and fork_of.slug == 'picross' and repo.owner != 'fvogels'
end

forks_of_picross.each do |repo|
  if not $logins.has_key? repo.owner
  then abort "Unknown user #{repo.owner} in #{repo}"
  end

  student_id = $logins[repo.owner]

  puts <<-END
if [ ! -d #{student_id} ]; then
  git clone https://fvogels@bitbucket.org/#{repo.owner}/#{repo.slug}.git #{student_id}
fi
  END
end

