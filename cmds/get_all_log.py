#https://gist.github.com/eldondevcg/fffff4b7909351b19a5

import boto3, json, time, os, sys
from botocore.config import Config
from datetime import datetime, timedelta, timezone
from contextlib import contextmanager
from pathlib import Path
@contextmanager
def set_directory(directory):
    origin = Path().absolute()
    try:
        if not os.path.exists(directory):
            os.makedirs(directory)
        os.chdir(directory)
        yield
    finally:
        os.chdir(origin)
my_config = Config(
    region_name = 'eu-west-2',
    signature_version = 'v4',
    retries = {
        'max_attempts': 10,
        'mode': 'standard'
    }
)
client = boto3.client('logs', aws_access_key_id="AKIAVHQXKLFGXCA5NVV7",
    aws_secret_access_key="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",config=my_config)
def print_consle(log):
    sys.stdout.write(f"{log}\r")
    sys.stdout.flush()
nowstr = None
def get_now_to_str():
    global nowstr
    if not nowstr:
        nowstr = datetime.now().strftime("%Y_%m_%d__%H%M%S")
    return nowstr

def is_last_event_after(timestamp_first_time_limit, stream_loggroup):
   return 'lastEventTimestamp' in stream_loggroup and timestamp_first_time_limit < int(stream_loggroup['lastEventTimestamp']) 

def get_logstream_for_log_group_name(log_group_name, first_time_ingestion):
    all_streams = []
    stream_batch = client.describe_log_streams(logGroupName=log_group_name, orderBy='LastEventTime', descending=True)
    all_streams += [stream_b  for stream_b in stream_batch['logStreams'] if is_last_event_after(first_time_ingestion, stream_b)] 
    while 'nextToken' in stream_batch and all_streams and is_last_event_after(first_time_ingestion, stream_batch['logStreams'][-1]):
        stream_batch = client.describe_log_streams(logGroupName=log_group_name,nextToken=stream_batch['nextToken'], orderBy='LastEventTime',descending=True)
        all_streams += [stream_b  for stream_b in stream_batch['logStreams'] if is_last_event_after(first_time_ingestion, stream_b)]  
        print(len(all_streams))
    stream_names = [stream['logStreamName'] for stream in all_streams]
    print(len(all_streams))
    print(stream_names)
    for stream_name in stream_names:
        yield log_group_name, stream_name

def get_logs_from_logstream(log_group_name_logstreams):
    for log_group_name, stream in log_group_name_logstreams:
        result = []
        logs_batch = client.get_log_events(logGroupName=log_group_name, logStreamName=stream, startFromHead=True)
        for event in logs_batch['events']:
            event.update({'group': log_group_name, 'stream':stream })
            result.append(event)
            # print(logs_batch)
        next_token = ''
        while 'nextForwardToken' in logs_batch and logs_batch['nextForwardToken'] != next_token:
            # print('nest token '+ logs_batch['nextForwardToken'])
            next_token = logs_batch['nextForwardToken']
            logs_batch = client.get_log_events(logGroupName=log_group_name, logStreamName=stream, nextToken=next_token)
            for event in logs_batch['events']:
                event.update({'group': log_group_name, 'stream':stream })
                result.append(event)
        # print(result)
        yield (stream, result)
def is_json(myjson):
    try:
        json.loads(myjson)
        return "Microsoft.AspNetCore.DataProtection" not in json.dumps(myjson)
    except ValueError as e:
        return False
    return True

def extract_message(event):
    if not 'message' in event: return None
    raw_data = event['message']
    if not is_json(raw_data): return None
    return json.dumps(json.loads(raw_data), indent=4)+",\n"


def _write_log_event_to_one_files(logstream_names_events):
    file_name = f'result_one_{get_now_to_str()}.json'
    with open(file_name, 'w') as out_to:
        for logstream_name, events in logstream_names_events:
            for event in events:
                out_to.write(json.dumps(event)+",\n")
                out_to.flush()
                # yield event
            yield (logstream_name, events)

def write_log_event_to_file(logstream_names_events):
    return _write_log_event_to_one_files(_write_log_event_to_one_files(logstream_names_events))
    # return _write_log_event_every_x_to_one_files(1000,_write_log_event_to_multiple_files(_write_log_event_to_one_files(logstream_names_events)))
    # return _write_log_event_to_one_files(logstream_names_events)
    # return _write_log_event_to_multiple_files(logstream_names_events)

def print_regex(regex_pattern, filename_part, logstream_names_events):
    # if not os.path.exists(result_found_folder):
    #         os.makedirs(result_found_folder)
    with open(f'resul_regex_{filename_part}_{get_now_to_str()}.json','w') as f_reg:
        for logstream_name, events in logstream_names_events:
            for event in events:
                if regex_pattern.lower() in str(event).lower():
                    print(f"####\nfound it: {event}")
                    f_reg.write(json.dumps(event)+",\n")
            yield (logstream_name, events)

def get_message(logstream_names_events):
    file_name = f'result_message_{get_now_to_str()}.json'
    with open(file_name,'w') as f_mes:
        for logstream_name, events in logstream_names_events:
            for event in events:
                if extract_message(event):
                    f_mes.write(extract_message(event))
            yield (logstream_name, events)

WeddingLoggroup = [
    "/aws/lambda/WeddingWebsite-AspNetCoreFunction-ukWHvqtZ2DMI",
]

results_folder = "results_player_" + get_now_to_str()
result_folder_found = "found"
log_groups_to_check = WeddingLoggroup
first_time_ingestion =  datetime(2022, 11, 8, 0, 0, 0).timestamp() * 1000
def main():
    input_loggroups = log_groups_to_check
    for input_loggroup in input_loggroups:
        print('*'*100)
        print(input_loggroup)
        with set_directory(results_folder):
            # list(write_log_event_to_file(get_logs_from_logstream(get_logstream_for_log_group_name(input_loggroup))))
            list(get_message(print_regex('error', 'error',write_log_event_to_file(get_logs_from_logstream(get_logstream_for_log_group_name(input_loggroup, first_time_ingestion))))))

if __name__ == "__main__":
    main()